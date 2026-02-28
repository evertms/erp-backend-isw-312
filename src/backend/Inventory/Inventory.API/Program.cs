using Inventory.API.Shared.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.API.Features.Companies.Application;
using Inventory.API.Features.Companies.InterfaceAdapters;
using Inventory.API.Features.Dashboard.Application;
using Inventory.API.Features.Dashboard.InterfaceAdapters;
using Inventory.API.Features.Inventory.Application;
using Inventory.API.Features.Inventory.InterfaceAdapters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Host=localhost;Database=inventory_db;Username=postgres;Password=Password123!"));

// Register Handlers (Application Layer)
builder.Services.AddScoped<GetActiveCompaniesHandler>();
builder.Services.AddScoped<GetCompanyWarehousesHandler>();
builder.Services.AddScoped<GetCompanyProductsHandler>();
builder.Services.AddScoped<GetDashboardMetricsHandler>();
builder.Services.AddScoped<CreateStockAdjustmentHandler>();
builder.Services.AddScoped<GetProductStockHandler>();
builder.Services.AddScoped<GetProductStockInWarehouseHandler>();

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
}

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// Map Feature Endpoints
app.MapCompaniesEndpoints();
app.MapDashboardEndpoints();
app.MapInventoryEndpoints();

app.Run();