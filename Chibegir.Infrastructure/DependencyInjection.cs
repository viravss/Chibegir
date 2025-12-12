using Chibegir.Application.Interfaces;
using Chibegir.Application.Services;
using Chibegir.Domain.Entities;
using Chibegir.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Chibegir.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register repositories
        services.AddSingleton<IRepository<Product>, InMemoryRepository<Product>>();

        // Register application services
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}

