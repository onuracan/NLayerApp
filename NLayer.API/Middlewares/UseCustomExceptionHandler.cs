using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Net;

namespace NLayer.API.Middlewares;

public static class UseCustomExceptionHandler
{
    public static void UseCustomException(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(cfg =>
        {
            cfg.Run(async (context) =>
            {
                context.Response.ContentType = "application/json";
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                context.Response.StatusCode = exceptionFeature.Error switch
                {
                    ClientSideException => 400,
                    NotFoundException => 404,
                    _ => 500
                };
                var response = new CustomResponseDto<NoContentDto>() { StatusCode = (HttpStatusCode)context.Response.StatusCode, Errors = new List<string>() { exceptionFeature.Error.Message.ToString() } };

                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize<CustomResponseDto<NoContentDto>>(response));
            });
        });
    }
}
