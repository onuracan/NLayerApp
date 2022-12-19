using Microsoft.EntityFrameworkCore;
using NLayer.Core.Model;
using NLayer.Repository.Configurations;
using System.Reflection;

namespace NLayer.Repository;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductFeature> ProductFeatures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //modelBuilder.ApplyConfiguration<Product>(new ProductConfiguration());
        //new ProductConfiguration().Configure(modelBuilder.Entity<Product>());

        //modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
        //{

        //});


        base.OnModelCreating(modelBuilder);
    }
}
