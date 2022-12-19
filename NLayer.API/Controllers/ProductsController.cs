using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Model;
using NLayer.Core.Services;

namespace NLayer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : CustomBaseController
{
    private readonly IMapper _mapper;
    private readonly IProductService _service;

    public ProductsController(IProductService service, IMapper mapper)
    {
        this._service = service;
        this._mapper = mapper;
    }

    public async Task<IActionResult> All()
    {
        var products = await _service.GetAllAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products.ToList());

        return base.CreateActionResult<List<ProductDto>>(CustomResponseDto<List<ProductDto>>.Success(System.Net.HttpStatusCode.OK, productDtos));
    }

    [HttpGet("GetProductsWithCategory")]
    public async Task<IActionResult> GetProductsWithCategory(int id)
    {
        return base.CreateActionResult(await this._service.GetProductsWithCategory());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);

        var productDto = _mapper.Map<ProductDto>(product);

        return base.CreateActionResult<ProductDto>(CustomResponseDto<ProductDto>.Success(System.Net.HttpStatusCode.OK, productDto));
    }

    [HttpPost]
    public async Task<IActionResult> Save(ProductDto productDto)
    {
        var product = await _service.AddAsync(this._mapper.Map<Product>(productDto));

        var productsDto = _mapper.Map<ProductDto>(product);

        return base.CreateActionResult<ProductDto>(CustomResponseDto<ProductDto>.Success(System.Net.HttpStatusCode.OK, productsDto));
    }

    [HttpPut]
    public async Task<IActionResult> Update(ProductDto productDto)
    {
        await _service.UpdateAsync(this._mapper.Map<Product>(productDto));

        return base.CreateActionResult<ProductDto>(CustomResponseDto<ProductDto>.Success(System.Net.HttpStatusCode.NoContent, null));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var product = await this._service.GetByIdAsync(id);
        await _service.DeleteAsync(product);

        return base.CreateActionResult(CustomResponseDto<NoContentDto>.Success(System.Net.HttpStatusCode.NoContent, null));
    }
}