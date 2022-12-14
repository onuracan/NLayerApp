using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        this._context = context;
        this._dbSet = this._context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await this._dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await this._dbSet.AddRangeAsync(entities);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await this._dbSet.AnyAsync(expression);
    }

    public IQueryable<T> GetAll()
    {
        return this._dbSet.AsNoTracking().AsQueryable();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await this._dbSet.FindAsync(id);
    }

    public void Delete(T entity)
    {
        this._dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        this._dbSet.RemoveRange(entities);
    }

    public void Update(T entity)
    {
        this._dbSet.Update(entity);
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return this._dbSet.Where(expression);
    }
}
