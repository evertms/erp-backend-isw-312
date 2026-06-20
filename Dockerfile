FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG PROJECT_PATH
WORKDIR /src

# Copy the entire solution and source code
COPY . .

# Restore and publish the specific project
RUN dotnet restore ${PROJECT_PATH}
RUN dotnet publish ${PROJECT_PATH} -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose standard port for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
