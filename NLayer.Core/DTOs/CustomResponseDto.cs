using System.Net;
using System.Text.Json.Serialization;

namespace NLayer.Core.DTOs;

public class CustomResponseDto<T>
{
    public T Data { get; set; }

    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }

    public List<string> Errors { get; set; }

    public static CustomResponseDto<T> Success(HttpStatusCode statusCode, T data)
    {
        return new CustomResponseDto<T>() { StatusCode = statusCode, Data = data };
    }

    public static CustomResponseDto<T> Fail(HttpStatusCode statusCode, List<string> errors)
    {
        return new CustomResponseDto<T>() { StatusCode = statusCode, Errors = errors };
    }
}