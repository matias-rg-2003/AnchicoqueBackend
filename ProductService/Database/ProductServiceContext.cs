namespace ProductService.Database;

using Microsoft.EntityFrameworkCore;
using ProductService.Models;

public class ProductServiceContext(DbContextOptions<ProductServiceContext> options) : DbContext(options)
{
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Line> Lines { get; set; }
    public DbSet<Leather> Leathers { get; set; }
    public DbSet<Category> Categories { get; set; }
    //agregar las demas entidades
}
