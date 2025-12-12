using Chibegir.Application.DTOs;

namespace Chibegir.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken = default);
    Task<ProductDto> UpdateProductAsync(Guid id, ProductDto productDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
}

