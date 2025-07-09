namespace UserService.Database;

using Microsoft.EntityFrameworkCore;
using UserService.Models;

public class UserServiceContext(DbContextOptions<UserServiceContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}