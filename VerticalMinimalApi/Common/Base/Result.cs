using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace VerticalMinimalApi.Common.Base;

public class Result<T>: IResult
{
    public T? Data { get; set; }
    public IEnumerable<Error.Error>? Errors { get; set; }

    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// A helper method that returns a shallow copy of <see cref="Value"/> and a deep copy of <see cref="Errors"/> without the <see cref="Error.Message"/>
    /// </summary>
    /// <returns></returns>
    public Result<T> GeneratePublicResult()
    {
        var newResult = new Result<T>
        {
            Errors = Errors?.Select(error => new Error.Error()
            {
                Message = error.Message,
                DisplayMessage = error.DisplayMessage,
                ErrorCode = error.ErrorCode
            }),
            Data = Data
        };
        return newResult;
    }

    public static Result<T> CreateSuccess(T result, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new Result<T> { Data = result, StatusCode = statusCode };
    }

    public static Result<T> CreateFailure(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T> { Errors = new[] { new Error.Error { Message = errorMessage } }, StatusCode = statusCode };
    }

    public static Result<T> CreateFailure(string errorMessage, string displayMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T> { Errors = new[] { new Error.Error { Message = errorMessage, DisplayMessage = displayMessage } } , StatusCode = statusCode };
    }

    public static Result<T> CreateFailure(HttpStatusCode statusCode = HttpStatusCode.BadRequest, params Error.Error[] errors)
    {
        return new Result<T> {  Errors = errors, StatusCode = statusCode };
    }
    // public static Result<T> CreateFailure(Exception exception, params Error.Error[] errors)
    // {
    //     return new Result<T> {  Errors = errors };
    // }

    public static Result<T> CreateFailure(Exception exception, string? displayMessage = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Error.Error error = Error.Error.FromException(exception, displayMessage);
        return new Result<T>() { Errors = new[] { error }, StatusCode = statusCode};
    }
    
    public JsonHttpResult<Result<T>> Response()
    {
        return TypedResults.Json(this, statusCode: (int)StatusCode);
    }
    //
    // public static Result<T> CreateFailure(IResult result)
    // {
    //     if (!result.Errors.Any())
    //     {
    //         result.Errors = new[] { new Error.Error { DisplayMessage = result.Message, Message = result.Message } };
    //     }
    //     return CreateFailure( result.StatusCode, result.Errors.ToArray());
    // }
}