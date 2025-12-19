using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;
using Chibegir.Infrastructure.Data;
using Chibegir.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chibegir.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.RegisterDbContext(configuration);

        // Register repositories for int-based entities (using DbContext)
        services.AddScoped<IRepositoryInt<Product>, RepositoryInt<Product>>();
        services.AddScoped<IRepositoryInt<Source>, RepositoryInt<Source>>();
        services.AddScoped<IRepositoryInt<ProductSource>, RepositoryInt<ProductSource>>();
        services.AddScoped<IRepositoryInt<ProductLog>, RepositoryInt<ProductLog>>();

        // Register service implementations (interfaces are defined in Application layer)
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISourceService, SourceService>();
        services.AddScoped<IProductSourceService, ProductSourceService>();
        services.AddScoped<IProductLogService, ProductLogService>();

        return services;
    }

    private static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        
        // Register DbContext
        var connectionString = configuration.GetConnectionString("ChiBazConnection");

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(connectionString)
            );
    }
}

