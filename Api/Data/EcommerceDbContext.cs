using Microsoft.EntityFrameworkCore;
using ECommerceApi.Models;

public class EcommerceDb : DbContext
{
    public EcommerceDb(DbContextOptions<EcommerceDb> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserCart)
            .WithOne(c => c.CartUser)
            .HasForeignKey<Cart>(c => c.CartId);

        modelBuilder.Entity<CartItem>()
            .HasKey(ci => ci.CartItemId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.CartItemCart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.CartItemProduct)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId);
    }
}
