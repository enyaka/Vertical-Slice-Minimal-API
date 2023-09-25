using System.Reflection;
using System.Text.Json.Serialization;
using Carter;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Serilog;
using VerticalMinimalApi.Common.Middlewares;
using VerticalMinimalApi.Context;
using VerticalMinimalApi.Options;
using VerticalMinimalApi.Repositories.User;

namespace VerticalMinimalApi.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection MapServices(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration).CreateLogger();
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)));
        services.AddDbContext<MinimalDbContext>();
        services.ConfigureHttpJsonOptions(options => options.SerializerOptions.DefaultIgnoreCondition
            = JsonIgnoreCondition.WhenWritingNull);


        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(typeAdapterConfig));

        services.AddCarter();

        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}