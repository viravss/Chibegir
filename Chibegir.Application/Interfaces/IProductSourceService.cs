using Chibegir.Application.DTOs;

namespace Chibegir.Application.Interfaces;

public interface IProductSourceService
{
    Task<ProductSourceDto?> GetProductSourceByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSourceDto>> GetAllProductSourcesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSourceDto>> GetProductSourcesByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSourceDto>> GetProductSourcesBySourceIdAsync(int sourceId, CancellationToken cancellationToken = default);
    Task<ProductSourceDto> CreateProductSourceAsync(ProductSourceDto productSourceDto, CancellationToken cancellationToken = default);
    Task<ProductSourceDto> UpdateProductSourceAsync(int id, ProductSourceDto productSourceDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductSourceAsync(int id, CancellationToken cancellationToken = default);
}
