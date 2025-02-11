using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SikiSokoChatApp.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions
        {
            // Allow positive and negative infinity literals
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        });
        //services.AddScoped<IGenderService, GenderService>();
        
        services.AddHttpClient();

        return services;
    }
}