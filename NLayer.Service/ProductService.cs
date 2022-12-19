using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Model;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWork;

namespace NLayer.Service;

public class ProductService : Service<Product>, IProductService
{
    private readonly IProductRepository _repositoryProduct;
    private readonly IMapper _mapper;

    public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IProductRepository repositoryProduct, IMapper mapper) : base(repository, unitOfWork)
    {
        this._repositoryProduct = repositoryProduct;
        this._mapper = mapper;
    }

    public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
    {
        var product = await this._repositoryProduct.GetProductsWithCategory();

        return CustomResponseDto<List<ProductWithCategoryDto>>.Success(System.Net.HttpStatusCode.OK, this._mapper.Map<List<ProductWithCategoryDto>>(product));
    }
}
