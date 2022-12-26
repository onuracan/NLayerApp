using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Model;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWork;
using NLayer.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching;

public class ProductServiceWithCaching : IProductService
{
    private const string CacheProductKey = "productsCache";
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductServiceWithCaching(IUnitOfWork unitOfWork, IProductRepository repository, IMemoryCache memoryCache, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._repository = repository;
        this._memoryCache = memoryCache;
        this._mapper = mapper;

        if (!this._memoryCache.TryGetValue(CacheProductKey, out _))
        {
            this._memoryCache.Set(CacheProductKey, this._repository.GetAll().ToList());
        }
    }

    public async Task<Product> AddAsync(Product entity)
    {
        await _repository.AddAsync(entity);
        await _unitOfWork.CommitAsync();

        await this.CacheAllProducts();

        return entity;
    }

    public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
    {
        await _repository.AddRangeAsync(entities);
        await _unitOfWork.CommitAsync();

        await this.CacheAllProducts();

        return entities;
    }

    public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Product entity)
    {
        _repository.Delete(entity);

        await _unitOfWork.CommitAsync();

        await this.CacheAllProducts();
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult(this._memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
    }

    public Task<Product> GetByIdAsync(int id)
    {
        var product = this._memoryCache.Get<List<Product>>(CacheProductKey).Where(x => x.Id == id).FirstOrDefault();
        if (product is null)
        {
            throw new NotFoundException($"{typeof(Product).Name}({id}) not found");
        }

        return Task.FromResult(product);
    }

    public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
    {
        var products = await _repository.GetProductsWithCategory();

        var productsCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

        return await Task.FromResult(new CustomResponseDto<List<ProductWithCategoryDto>>() { Data = productsCategoryDto, StatusCode = System.Net.HttpStatusCode.OK });
    }

    public async Task RemoveRangeAsync(IEnumerable<Product> entities)
    {
        _repository.RemoveRange(entities);

        await _unitOfWork.CommitAsync();

        await this.CacheAllProducts();
    }

    public async Task UpdateAsync(Product entity)
    {
        _repository.Update(entity);

        await _unitOfWork.CommitAsync();

        await this.CacheAllProducts();
    }

    public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
    {
        //return this._memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile().AsQueryable());
        return null;
    }

    public async Task CacheAllProducts()
    {
        this._memoryCache.Set(CacheProductKey, this._repository.GetAll().ToList());
    }

    public async Task<List<ProductWithCategoryDto>> GetProductsWithCategoryForMvc()
    {
        var products = await _repository.GetProductsWithCategory();

        var productsCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

        return await Task.FromResult(productsCategoryDto);
    }
}
