using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerticalMinimalApi.Common.Base;
using VerticalMinimalApi.Context;
using VerticalMinimalApi.Models;
using VerticalMinimalApi.Options;

var builder = WebApplication.CreateBuilder(args); {
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
    builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
    builder.Services.AddDbContext<MinimalDbContext>();
    
    
}



var app = builder.Build(); {
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapGet("/api", async Task<JsonHttpResult<BaseResponse<User>>> (MinimalDbContext context, CancellationToken ct) =>
    {
        var users = await context.Users.ToListAsync(ct);
        var user = users.First();
        var random = new Random();
        var rand = random.Next(100);
        var sayi = rand % 2;
        var result = BaseResponse<User>.Success(user);
        if (sayi == 0) return result.Response();
        result = BaseResponse<User>.Failure(new string[] { "Invalid" });
        return result.Response();
    });
    
    app.Run();
}
