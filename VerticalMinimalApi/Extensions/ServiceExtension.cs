using System.Reflection;
using System.Text.Json.Serialization;
using Carter;
using Mapster;
using MapsterMapper;
using VerticalMinimalApi.Context;
using VerticalMinimalApi.Options;
using VerticalMinimalApi.Repositories.User;

namespace VerticalMinimalApi.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection MapServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
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

