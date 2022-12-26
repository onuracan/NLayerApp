using Microsoft.EntityFrameworkCore;
using NLayer.Core.Model;
using NLayer.Repository.Configurations;
using System.Reflection;

namespace NLayer.Repository;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var item in ChangeTracker.Entries())
        {
            if (item.Entity is BaseEntity entityReference)
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        {
                            entityReference.CreatedDate = DateTime.Now;
                        }
                        break;
                    case EntityState.Modified:
                        {
                            Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                            entityReference.UpdatedDate = DateTime.Now;
                        }
                        break;
                }
            }
        }


        return base.SaveChangesAsync(cancellationToken);
    }
}
