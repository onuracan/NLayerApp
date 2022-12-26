using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using System.Net;

namespace NLayer.Web.Filters;

public class ValidateFilterAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            context.Result = new BadRequestObjectResult(new CustomResponseDto<NoContentDto>() { Errors = errors, StatusCode = HttpStatusCode.NoContent });
        }
    }
}
