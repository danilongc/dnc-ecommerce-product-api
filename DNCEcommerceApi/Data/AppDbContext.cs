using DNCEcommerceApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DNCEcommerceApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasAlternateKey(c => c.Sku)
            .HasName("AlternateKey_Sku");

    }

}

