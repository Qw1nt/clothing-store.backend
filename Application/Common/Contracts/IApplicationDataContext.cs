using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Contracts;

public interface IApplicationDataContext
{
    public DbSet<User> Users { get; }
    
    public DbSet<CartItem> CartItems { get; }
    
    public DbSet<Product> Products { get; }
    
    public DbSet<Category> Categories { get; }
    
    public DbSet<Review> Reviews { get; }
    
    public DbSet<Order> Orders { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}