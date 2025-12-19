using Chibegir.Domain.Entities;

namespace Chibegir.Application.Interfaces;

public interface IProductSourceRepository : IRepositoryInt<ProductSource>
{
    Task<ProductSource?> GetByIdWithRelationsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSource>> GetAllWithRelationsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSource>> GetByProductIdWithRelationsAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSource>> GetBySourceIdWithRelationsAsync(int sourceId, CancellationToken cancellationToken = default);
}
