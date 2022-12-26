using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<AppDbContext> Builder = null;

        try
        {
            Builder = new DbContextOptionsBuilder<AppDbContext>();

            Builder.UseSqlServer("Server=DeveloperOnur;Database=NLayerDb;Trusted_Connection=True;MultipleActiveResultSets=true;");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return new AppDbContext(Builder.Options);
    }
}
