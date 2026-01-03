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

        // Register generic repositories for int-based entities (using DbContext)
        services.AddScoped<IRepositoryInt<Product>, RepositoryInt<Product>>();
        services.AddScoped<IRepositoryInt<Source>, RepositoryInt<Source>>();
        services.AddScoped<IRepositoryInt<ProductSource>, RepositoryInt<ProductSource>>();
        services.AddScoped<IRepositoryInt<ProductLog>, RepositoryInt<ProductLog>>();
        services.AddScoped<IRepositoryInt<Category>, RepositoryInt<Category>>();
        services.AddScoped<IRepositoryInt<Domain.Entities.Attribute>, RepositoryInt<Domain.Entities.Attribute>>();
        services.AddScoped<IRepositoryInt<CategoryAttribute>, RepositoryInt<CategoryAttribute>>();

        // Register specific repositories with Include/Join support
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductSourceRepository, ProductSourceRepository>();
        services.AddScoped<IProductLogRepository, ProductLogRepository>();
        services.AddScoped<ICategoryAttributeRepository, CategoryAttributeRepository>();

        // Register service implementations (interfaces are defined in Application layer)
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISourceService, SourceService>();
        services.AddScoped<IProductSourceService, ProductSourceService>();
        services.AddScoped<IProductLogService, ProductLogService>();
        services.AddScoped<IMongoProductService, MongoProductService>();
        services.AddScoped<IAttributeService, AttributeService>();
        services.AddScoped<ICategoryAttributeService, CategoryAttributeService>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddHttpClient();

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

