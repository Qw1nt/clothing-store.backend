using ClothingStore.Data.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Data.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(options => options.MigrationsAssembly("ClothingStore.API"));
    }
    
    public DbSet<User> Users => Set<User>();

    public DbSet<CartItem> CartItems => Set<CartItem>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Review> Reviews => Set<Review>();

    public DbSet<Order> Orders => Set<Order>();
}