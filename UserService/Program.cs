using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Services.Implementations;
using UserService.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<UserServiceContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

//Registro de Servicios
builder.Services.AddScoped<IUserService, UserServiceImpl>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceContext>();
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