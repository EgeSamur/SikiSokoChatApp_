using Amazon.Rekognition;
using Amazon.Runtime;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Infrastructure.Persistence.Contexts;
using SikiSokoChatApp.Infrastructure.Persistence.Interceptors;
using SikiSokoChatApp.Infrastructure.Persistence.Repositories;
using SikiSokoChatApp.Infrastructure.Persistence.Repositories.Base;

namespace SikiSokoChatApp.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, EntitySaveChangesInterceptor>();
        // ConnectionString'den DataSource olu�turma
        var connectionString = configuration.GetConnectionString("Default");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNetTopologySuite(); // NetTopologySuite deste�ini ekliyoruz
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(dataSource, npgsqlOptions =>
            {
                npgsqlOptions.UseNetTopologySuite(); // NetTopologySuite'i kullan�yoruz
            });
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //services.AddScoped<IUserRepository, UserRepository>();
       
        return services;
    }
}