using Chibegir.Domain.Entities;

namespace Chibegir.Application.Interfaces;

public interface IProductLogRepository : IRepositoryInt<ProductLog>
{
    Task<ProductLog?> GetByIdWithRelationsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductLog>> GetAllWithRelationsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductLog>> GetByProductIdWithRelationsAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductLog>> GetBySourceIdWithRelationsAsync(int sourceId, CancellationToken cancellationToken = default);
}
