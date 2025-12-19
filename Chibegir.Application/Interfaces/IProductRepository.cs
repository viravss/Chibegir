using Chibegir.Domain.Entities;

namespace Chibegir.Application.Interfaces;

public interface IProductRepository : IRepositoryInt<Product>
{
    Task<Product?> GetByIdWithSourceAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllWithSourcesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAvailableWithSourcesAsync(CancellationToken cancellationToken = default);
}
