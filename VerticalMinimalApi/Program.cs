using Carter;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VerticalMinimalApi.Common.Base;
using VerticalMinimalApi.Common.Error;
using VerticalMinimalApi.Context;
using VerticalMinimalApi.Extensions;
using VerticalMinimalApi.Features.Users;
using VerticalMinimalApi.Features.Users.Common;
using VerticalMinimalApi.Models;

var builder = WebApplication.CreateBuilder(args); {
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.MapServices(builder.Configuration);
}



var app = builder.Build(); {
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapCarter();
    
    app.Run();
}
