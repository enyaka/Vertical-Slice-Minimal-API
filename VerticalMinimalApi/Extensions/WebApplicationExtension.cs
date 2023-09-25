using System.Net;
using System.Text.Json;
using Carter;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using VerticalMinimalApi.Common.Base;
using VerticalMinimalApi.Common.Error;

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
                var response = BaseResponse<EmptyResult>.Failure(BaseError.New("Server error", exception?.Message ?? "", exception?.InnerException?.Message).ToList(),HttpStatusCode.InternalServerError);
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var result = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(result);
            });
        });
        return app;
    }
}