using Chibegir.Application.DTOs.NoSqlSchema;
using Chibegir.Application.Enums;

namespace Chibegir.Application.Interfaces;

public interface IMongoProductService
{
    Task<string> InsertProductAsync(ProductSchema product, CancellationToken cancellationToken = default);
    Task<ProductSchema?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductSchema?> GetProductByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSchema>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSchema>> SearchProductsByTitleAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSchema>> GetProductsByBrandAsync(string brandName, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSchema>> GetProductsByCategoryAsync(ProductCategoryEnum category, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSchema>> GetAvailableProductsAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateProductAsync(int id, ProductSchema product, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default);
}

