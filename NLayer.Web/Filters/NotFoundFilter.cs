using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Model;
using NLayer.Core.Services;

namespace NLayer.Web.Filters;

public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
{
    private readonly IService<T> _service;

    public NotFoundFilter(IService<T> service)
    {
        this._service = service;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var idValue = context.ActionArguments.Values.FirstOrDefault();

        if (idValue is null)
        {
            await next.Invoke();

            return;
        }

        var id = (int)idValue;

        var anyEntinty = await this._service.AnyAsync(x => x.Id == id);
        if (anyEntinty is true)
        {
            await next.Invoke();
            return;
        }

        var error = new ErrorViewModel();
        error.Errors.Add($"{typeof(T).Name}({id}) not found");
        context.Result = new RedirectToActionResult("Error", "Home", error);

    }
}
