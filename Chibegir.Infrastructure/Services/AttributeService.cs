using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using AttributeEntity = Chibegir.Domain.Entities.Attribute;

namespace Chibegir.Infrastructure.Services;

public class AttributeService : IAttributeService
{
    private readonly IRepositoryInt<AttributeEntity> _attributeRepository;

    public AttributeService(IRepositoryInt<AttributeEntity> attributeRepository)
    {
        _attributeRepository = attributeRepository;
    }

    public async Task<AttributeDto?> GetAttributeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var attribute = await _attributeRepository.GetByIdAsync(id, cancellationToken);
        return attribute == null ? null : MapToDto(attribute);
    }

    public async Task<IEnumerable<AttributeDto>> GetAllAttributesAsync(CancellationToken cancellationToken = default)
    {
        var attributes = await _attributeRepository.GetAllAsync(cancellationToken);
        return attributes.Select(MapToDto);
    }

    public async Task<AttributeDto?> GetAttributeByKeyAsync(string attributeKey, CancellationToken cancellationToken = default)
    {
        var attributes = await _attributeRepository.FindAsync(a => a.AttributeKey == attributeKey, cancellationToken);
        return attributes.FirstOrDefault() != null ? MapToDto(attributes.First()) : null;
    }

    public async Task<IEnumerable<AttributeDto>> GetAttributesByTypeAsync(string attributeType, CancellationToken cancellationToken = default)
    {
        var attributes = await _attributeRepository.FindAsync(a => a.AttributeType == attributeType, cancellationToken);
        return attributes.Select(MapToDto);
    }

    public async Task<AttributeDto> CreateAttributeAsync(AttributeDto attributeDto, CancellationToken cancellationToken = default)
    {
        var attribute = MapToEntity(attributeDto);
        attribute = await _attributeRepository.AddAsync(attribute, cancellationToken);
        return MapToDto(attribute);
    }

    public async Task<AttributeDto> UpdateAttributeAsync(int id, AttributeDto attributeDto, CancellationToken cancellationToken = default)
    {
        var existingAttribute = await _attributeRepository.GetByIdAsync(id, cancellationToken);
        if (existingAttribute == null)
            throw new KeyNotFoundException($"Attribute with id {id} not found.");

        existingAttribute.AttributeKey = attributeDto.AttributeKey;
        existingAttribute.Label = attributeDto.Label;
        existingAttribute.AttributeType = attributeDto.AttributeType;
        existingAttribute.Unit = attributeDto.Unit;
        existingAttribute.ModifiedOn = DateTime.UtcNow;

        await _attributeRepository.UpdateAsync(existingAttribute, cancellationToken);
        return MapToDto(existingAttribute);
    }

    public async Task<bool> DeleteAttributeAsync(int id, CancellationToken cancellationToken = default)
    {
        var attribute = await _attributeRepository.GetByIdAsync(id, cancellationToken);
        if (attribute == null)
            return false;

        await _attributeRepository.DeleteAsync(attribute, cancellationToken);
        return true;
    }

    private static AttributeDto MapToDto(AttributeEntity attribute)
    {
        return new AttributeDto
        {
            Id = attribute.Id,
            AttributeKey = attribute.AttributeKey,
            Label = attribute.Label,
            AttributeType = attribute.AttributeType,
            Unit = attribute.Unit,
            CreatedOn = attribute.CreatedOn,
            ModifiedOn = attribute.ModifiedOn
        };
    }

    private static AttributeEntity MapToEntity(AttributeDto attributeDto)
    {
        return new AttributeEntity
        {
            Id = attributeDto.Id,
            AttributeKey = attributeDto.AttributeKey,
            Label = attributeDto.Label,
            AttributeType = attributeDto.AttributeType,
            Unit = attributeDto.Unit
        };
    }
}

