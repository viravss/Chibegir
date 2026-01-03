using Chibegir.Application.DTOs;

namespace Chibegir.Application.Interfaces;

public interface IAttributeService
{
    Task<AttributeDto?> GetAttributeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AttributeDto>> GetAllAttributesAsync(CancellationToken cancellationToken = default);
    Task<AttributeDto?> GetAttributeByKeyAsync(string attributeKey, CancellationToken cancellationToken = default);
    Task<IEnumerable<AttributeDto>> GetAttributesByTypeAsync(string attributeType, CancellationToken cancellationToken = default);
    Task<AttributeDto> CreateAttributeAsync(AttributeDto attributeDto, CancellationToken cancellationToken = default);
    Task<AttributeDto> UpdateAttributeAsync(int id, AttributeDto attributeDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAttributeAsync(int id, CancellationToken cancellationToken = default);
}

