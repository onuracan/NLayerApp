using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service;

public class Service<T> : IService<T> where T : class
{
    private readonly IGenericRepository<T> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public Service(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
    {
        this._repository = repository;
        this._unitOfWork = unitOfWork;
    }


    public async Task<T> AddAsync(T entity)
    {
        await this._repository.AddAsync(entity);

        await this._unitOfWork.CommitAsync();

        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await this._repository.AddRangeAsync(entities);

        await this._unitOfWork.CommitAsync();

        return entities;
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await this._repository.AnyAsync(expression);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await this._repository.GetAll().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await this._repository.GetByIdAsync(id);
    }

    public async Task DeleteAsync(T entity)
    {
        this._repository.Delete(entity);

        await this._unitOfWork.CommitAsync();
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        this._repository.RemoveRange(entities);

        await this._unitOfWork.CommitAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        this._repository.Update(entity);

        await this._unitOfWork.CommitAsync();
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return this._repository.Where(expression);
    }
}
