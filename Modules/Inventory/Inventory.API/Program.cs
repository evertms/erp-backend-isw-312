using Inventory.API.Endpoints;
using Inventory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddInventoryModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // Ejecutar Migraciones y Seeders solo en entorno de desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseAuthorization();

// Map endpoints
app.MapProductEndpoints();
app.MapCategoryEndpoints();
app.MapUnitEndpoints();
app.MapInventoryEndpoints();
app.MapDashboardEndpoints();
app.MapWarehouseEndpoints();
app.MapCompanyEndpoints();

app.Run();