using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SikiSokoChatApp.Shared.CrossCuttingConcerns.Logging.Serilog.Logger;
using SikiSokoChatApp.Shared.CrossCuttingConcerns.Logging.Serilog;
using SikiSokoChatApp.Application.Abstractions.Services;
using SikiSokoChatApp.Application.Features.Users;
using SikiSokoChatApp.Application.Features;
using SikiSokoChatApp.Application.Features.FileStorage;

namespace SikiSokoChatApp.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSingleton<LoggerServiceBase, FileLogger>();
        services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions
        {
            // Allow positive and negative infinity literals
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        });
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IConversationService, ConversationsService>();
        services.AddScoped<IFileStorageService, FileStorageService>();

        services.AddHttpClient();

        return services;
    }
}