using Purchases.API.Endpoints;
using Purchases.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddPurchasesModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // Ejecutar Migraciones (opcional)
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<Purchases.Infrastructure.Persistence.PurchasesDbContext>();
        db.Database.Migrate();
    }
    
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors();
app.UseAuthorization();
app.UseHttpsRedirection();

// Map endpoints
app.MapPurchaseOrderEndpoints();
app.MapSupplierEndpoints();

app.Run();
