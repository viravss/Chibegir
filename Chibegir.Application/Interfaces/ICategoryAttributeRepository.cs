using Chibegir.Domain.Entities;

namespace Chibegir.Application.Interfaces;

public interface ICategoryAttributeRepository : IRepositoryInt<CategoryAttribute>
{
    Task<IEnumerable<CategoryAttribute>> GetByCategoryIdWithRelationsAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryAttribute>> GetByAttributeIdWithRelationsAsync(int attributeId, CancellationToken cancellationToken = default);
    Task<CategoryAttribute?> GetByCategoryAndAttributeAsync(int categoryId, int attributeId, CancellationToken cancellationToken = default);
}

