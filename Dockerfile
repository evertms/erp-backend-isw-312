FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
ARG PROJECT_PATH=Modules/Inventory/Inventory.API/Inventory.API.csproj
COPY . .
RUN dotnet restore ${PROJECT_PATH}
RUN dotnet publish ${PROJECT_PATH} -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

USER app
ENTRYPOINT ["dotnet", "Inventory.API.dll"]
