using NLayer.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        this._context = context;
    }

    public void Commit()
    {
        this._context.SaveChanges();
    }

    public async Task CommitAsync()
    {
        await this._context.SaveChangesAsync();
    }
}
