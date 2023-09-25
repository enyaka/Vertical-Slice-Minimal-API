using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using VerticalMinimalApi.Common.Base;
using VerticalMinimalApi.Common.Error;
using VerticalMinimalApi.Models;

namespace VerticalMinimalApi.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication MapWebApplications(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.MapCarter();
        app.ErrorHandler();

        return app;
    }

    private static WebApplication ErrorHandler(this WebApplication app)
    {
        app.UseExceptionHandler("/error");
        app.Map("/error", appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = Result<EmptyModel>.CreateFailure(HttpStatusCode.InternalServerError,
                    new Error { Message = exception?.Message ?? "Undefined exception", DisplayMessage = exception?.InnerException?.Message });
                // var response = BaseResponse<EmptyResult>.Failure(
                //     BaseError.New("Server error", exception?.Message ?? "", exception?.InnerException?.Message)
                //         .ToList(), HttpStatusCode.InternalServerError);
                // if (exception is ValidationException)
                // {
                //     var validationException = (ValidationException)exception;
                //     response = BaseResponse<EmptyResult>
                //         .Failure(validationException.Errors.Select(
                //                 error => BaseError.New(validationException.Message, error.ErrorMessage))
                //             .ToList());
                // }

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                };
                Log.Error("Global Error => {@message} \n {@details}", exception?.Message, exception?.InnerException);
                var result = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(result);
            });
        });
        return app;
    }
}