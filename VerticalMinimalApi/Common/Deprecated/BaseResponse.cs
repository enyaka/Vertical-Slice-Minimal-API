using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VerticalMinimalApi.Common.Error;

namespace VerticalMinimalApi.Common.Base;

public class BaseResponse<T> 
    where T: class
{
    private BaseResponse(T? data, ICollection<BaseError>? errors, HttpStatusCode statusCode)
    {
        Data = data;
        Errors = errors;
        StatusCode = statusCode;
    }

    public T? Data { get; private set; }
    public ICollection<BaseError>? Errors { get; private set; }

    [JsonIgnore] private HttpStatusCode StatusCode { get; }

    public static BaseResponse<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new BaseResponse<T>(data, null, statusCode);
    }

    public static BaseResponse<T> Failure(ICollection<BaseError> errors,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new BaseResponse<T>(null, errors, statusCode);
    }

    public JsonHttpResult<BaseResponse<T>> Response()
    {
        return TypedResults.Json(this, statusCode: (int)StatusCode);
    }
}