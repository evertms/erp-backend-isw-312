using Inventory.API.Endpoints;
using Inventory.API.Extensions;
using Inventory.Infrastructure;
using Core.Infrastructure;
using System.Threading.Channels;
using Inventory.Application.Features.Stocks.Events;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".env");
if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "erp_database";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

builder.Configuration["ConnectionStrings:DefaultConnection"] = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddCoreModule(builder.Configuration);
builder.Services.AddInventoryModule(builder.Configuration);

builder.Services.AddSingleton(Channel.CreateUnbounded<RestockEvent>());

var app = builder.Build();

if (true || app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // Ejecutar Migraciones y Seeders
    await app.ApplyMigrationsAndSeedAsync();
    
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors();
app.UseAuthorization();

// Map endpoints
app.MapProductEndpoints();
app.MapCategoryEndpoints();
app.MapUnitEndpoints();
app.MapInventoryEndpoints();
app.MapDashboardEndpoints();
app.MapWarehouseEndpoints();
app.MapCompanyEndpoints();
app.MapCoreModuleEndpoints();

app.Run();