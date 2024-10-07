using Microsoft.EntityFrameworkCore;
using ECommerceApi.Models;

public class EcommerceDb : DbContext
{
    public EcommerceDb(DbContextOptions<EcommerceDb> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.CartUser)
            .WithOne(u => u.UserCart)
            .HasForeignKey<Cart>(c => c.CartUserId);
    }
}
