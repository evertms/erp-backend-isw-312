using Core.Infrastructure;
using Inventory.Infrastructure;
using Sales.Infrastructure;
using Web.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Register modules
builder.Services.AddCoreModule(builder.Configuration);
builder.Services.AddInventoryModule(builder.Configuration);
builder.Services.AddSalesModule();

// Add CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173", "http://localhost:3001") // Common React/Vite ports
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // Ejecutar Migraciones y Seeders solo en entorno de desarrollo
    await app.ApplyMigrationsAndSeedAsync();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// Map modules endpoints
app.MapCoreModuleEndpoints();
app.MapInventoryModuleEndpoints();

app.Run();
