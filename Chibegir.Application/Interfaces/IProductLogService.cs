using Chibegir.Application.DTOs;

namespace Chibegir.Application.Interfaces;

public interface IProductLogService
{
    Task<ProductLogDto?> GetProductLogByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductLogDto>> GetAllProductLogsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductLogDto>> GetProductLogsByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductLogDto>> GetProductLogsBySourceIdAsync(int sourceId, CancellationToken cancellationToken = default);
    Task<ProductLogDto> CreateProductLogAsync(ProductLogDto productLogDto, CancellationToken cancellationToken = default);
}
