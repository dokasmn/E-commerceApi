using Microsoft.EntityFrameworkCore;
using ECommerceApi.Models;

public class EcommerceDb : DbContext
{
    public EcommerceDb(DbContextOptions<EcommerceDb> options)
        : base(options) { }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> User => Set<User>();
    public DbSet<Cart> Cart => Set<Cart>();
    public DbSet<CartItem> CartItem => Set<CartItem>();
}