using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
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

    [HttpGet]
    [Route("All")]
    public async Task<IActionResult> All()
    {
        var products = await _service.GetAllAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products.ToList());

        return new ObjectResult(new CustomResponseDto<List<ProductDto>>() { Data = productDtos, StatusCode = System.Net.HttpStatusCode.OK })
        {
            StatusCode = 200,
        };
    }

    [HttpGet]
    [Route("GetProductsWithCategory/{id}")]
    public async Task<IActionResult> GetProductsWithCategory(int id)
    {
        return new ObjectResult(await this._service.GetProductsWithCategory())
        {
            StatusCode = 200,
        };
    }

    // Sadece IAsyncActionFilter implement ettiği için ve attribute inheritance almadığı için ServiceFilter sınıfından yardım alarak tanımladık ve ayrıca ctor da parametre alıyor ise
    //ServiceFilter üzerinden kullanmam gerekiyor. Ayrıca servisi dependency injectiona eklemem gerekiyor.
    [ServiceFilter(typeof(NotFoundFilter<Product>))]
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);

        var productDto = _mapper.Map<ProductDto>(product);

        return new ObjectResult(new CustomResponseDto<ProductDto>() { Data = productDto, StatusCode = System.Net.HttpStatusCode.OK })
        {
            StatusCode = 200,
        };
    }

    [HttpPost]
    [Route("Save")]
    public async Task<IActionResult> Save(ProductDto productDto)
    {
        var product = await _service.AddAsync(this._mapper.Map<Product>(productDto));

        var productsDto = _mapper.Map<ProductDto>(product);

        return new ObjectResult(new CustomResponseDto<ProductDto>() { Data = productsDto, StatusCode = System.Net.HttpStatusCode.OK })
        {
            StatusCode = 200,
        };
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update(ProductDto productDto)
    {
        await _service.UpdateAsync(this._mapper.Map<Product>(productDto));

        return new ObjectResult(new CustomResponseDto<ProductDto>() { Data = null, StatusCode = System.Net.HttpStatusCode.NoContent })
        {
            StatusCode = 200,
        };
    }

    [HttpDelete]
    [Route("remove/{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var product = await this._service.GetByIdAsync(id);
        await _service.DeleteAsync(product);

        return new ObjectResult(new CustomResponseDto<NoContentDto>() { Data = null, StatusCode = System.Net.HttpStatusCode.NoContent })
        {
            StatusCode = 200,
        };
    }
}