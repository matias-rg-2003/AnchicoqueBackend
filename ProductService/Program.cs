using ProductService.Database;
using Microsoft.EntityFrameworkCore;
using ProductService.Services.Interfaces;
using ProductService.Services.Implementations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<ProductServiceContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//Registrar servicios
builder.Services.AddScoped<IProductService, ProductServiceImpl>();
builder.Services.AddScoped<ILineService, LineServiceImpl>();
builder.Services.AddScoped<ILeatherService, LeatherServiceImpl>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductServiceContext>();
    try
    {
        dbContext.Database.EnsureCreated();
        Console.WriteLine("✅ La base de datos está conectada y accesible.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error conectando a la base de datos: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
