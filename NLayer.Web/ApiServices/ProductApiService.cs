using NLayer.Core.DTOs;

namespace NLayer.Web.ApiServices;

public class ProductApiService
{
    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<List<ProductWithCategoryDto>> GetProductWithCategoryAsync()
    {
        var response = await this._httpClient.GetFromJsonAsync<CustomResponseDto<List<ProductWithCategoryDto>>>("products/All");

        return response.Data;
    }

    public async Task<ProductDto> SaveAsync(ProductDto product)
    {
        var response = await this._httpClient.PostAsJsonAsync("products/Save", product);

        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<ProductDto>>();

        return responseBody.Data;
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var response = await this._httpClient.GetFromJsonAsync<CustomResponseDto<ProductDto>>($"products/{id}");

        if (response.Errors.Any())
        {
            //...
        }

        return response.Data;
    }
}
