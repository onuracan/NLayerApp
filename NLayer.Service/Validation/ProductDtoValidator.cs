using FluentValidation;
using NLayer.Core.DTOs;


namespace NLayer.Service.Validation;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
	public ProductDtoValidator()
	{
		RuleFor(x => x.Name).NotNull().WithName("Ürün Adı").WithMessage("{PropertyName} Boş olmamalıdır.").NotEmpty().WithMessage("{PropertyName}");
		RuleFor(x => x.Price).InclusiveBetween(1, int.MaxValue).WithName("Fiyat").WithMessage("Belirlenmiş olmalıdır.");
		RuleFor(x => x.Stock).InclusiveBetween(1, int.MaxValue).WithName("Stok").WithMessage("Belirlenmiş olmalıdır.");
		RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithName("Kategori").WithMessage("Belirlenmiş olmalıdır.");
	}
}
