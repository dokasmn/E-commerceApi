using Microsoft.EntityFrameworkCore;

class EcommerceDb : DbContext
{
    public EcommerceDb(DbContextOptions<EcommerceDb> options)
        : base(options) { }
    public DbSet<Product> Products => Set<Product>();
}