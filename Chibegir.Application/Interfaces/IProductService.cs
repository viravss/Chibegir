using Chibegir.Application.DTOs;

namespace Chibegir.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetProductsBySourceIdAsync(int sourceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetAvailableProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDto> CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken = default);
    Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default);
}

