using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace VerticalMinimalApi.Common.Base;

public sealed class BaseResponse<T> where T: class
{
    public T? Data { get; private set; }
    public string[] Errors { get; private set; }

    [JsonIgnore]
    public HttpStatusCode StatusCode { get; private set; }
    
    private BaseResponse (T? data, string[] errors, HttpStatusCode statusCode)
    {
        Data = data;
        Errors = errors;
        StatusCode = statusCode;
    }

    public static BaseResponse<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new BaseResponse<T>(data, Array.Empty<string>(), statusCode: statusCode);
    }
    
    public static BaseResponse<T> Failure(string[] errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new BaseResponse<T>(null, errors, statusCode);
    }

    public JsonHttpResult<BaseResponse<T>> Response()
    {
        return TypedResults.Json(this, statusCode: (int)StatusCode);
    }
}