using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Model;
using NLayer.Core.Services;
using NLayer.Web.ApiServices;

namespace NLayer.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _serviceProduct;
    private readonly IService<Category> _serviceCategory;
    private readonly IMapper _mapper;

    private readonly ProductApiService _productApiService;

    public ProductsController(ProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    //public ProductsController(IProductService serviceProduct, IService<Category> serviceCategory, IMapper mapper)
    //{
    //    this._serviceProduct = serviceProduct;
    //    this._serviceCategory = serviceCategory;
    //    this._mapper = mapper;
    //}




    public async Task<IActionResult> Index()
    {
        //return View(await this._serviceProduct.GetProductsWithCategoryForMvc());

        return View(await _productApiService.GetProductWithCategoryAsync());
    }

    public async Task<IActionResult> Save()
    {
        var categories = await this._serviceCategory.GetAllAsync();

        var categoriesDto = this._mapper.Map<List<CategoryDto>>(categories.ToList());

        ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(ProductDto productDto)
    {
        if (ModelState.IsValid)
        {
            await this._serviceProduct.AddAsync(this._mapper.Map<Product>(productDto));

            return RedirectToAction(nameof(Index));
        }
        else
        {
            var categories = await this._serviceCategory.GetAllAsync();

            var categoriesDto = this._mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }
    }

    public async Task<IActionResult> Update(int id)
    {
        var product = await this._serviceProduct.GetByIdAsync(id);

        var categories = await this._serviceCategory.GetAllAsync();

        var categoriesDto = this._mapper.Map<List<CategoryDto>>(categories.ToList());

        ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", product.CategoryId);

        return View(this._mapper.Map<ProductDto>(product));
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProductDto productDto)
    {
        if (ModelState.IsValid)
        {
            await this._serviceProduct.UpdateAsync(this._mapper.Map<Product>(productDto));

            return RedirectToAction(nameof(Index));
        }
        else
        {
            var categories = await this._serviceCategory.GetAllAsync();

            var categoriesDto = this._mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }
    }


    public async Task<IActionResult> Delete(int id)
    {
        var product = await this._serviceProduct.GetByIdAsync(id);

        await this._serviceProduct.DeleteAsync(product);

        return RedirectToAction(nameof(Index));
    }
}
