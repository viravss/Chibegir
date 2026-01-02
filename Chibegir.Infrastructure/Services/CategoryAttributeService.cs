using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;
using AttributeEntity = Chibegir.Domain.Entities.Attribute;

namespace Chibegir.Infrastructure.Services;

public class CategoryAttributeService : ICategoryAttributeService
{
    private readonly ICategoryAttributeRepository _categoryAttributeRepository;

    public CategoryAttributeService(ICategoryAttributeRepository categoryAttributeRepository)
    {
        _categoryAttributeRepository = categoryAttributeRepository;
    }

    public async Task<CategoryAttributeDto?> GetCategoryAttributeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var categoryAttribute = await _categoryAttributeRepository.GetByIdAsync(id, cancellationToken);
        if (categoryAttribute == null)
            return null;

        // Load relations if not already loaded
        if (categoryAttribute.Category == null || categoryAttribute.Attribute == null)
        {
            var withRelations = await _categoryAttributeRepository.GetByCategoryIdWithRelationsAsync(categoryAttribute.CategoryId, cancellationToken);
            categoryAttribute = withRelations.FirstOrDefault(ca => ca.Id == id);
            if (categoryAttribute == null)
                return null;
        }

        return MapToDto(categoryAttribute);
    }

    public async Task<IEnumerable<CategoryAttributeDto>> GetAllCategoryAttributesAsync(CancellationToken cancellationToken = default)
    {
        var categoryAttributes = await _categoryAttributeRepository.GetAllAsync(cancellationToken);
        // Load relations for all
        var categoryIds = categoryAttributes.Select(ca => ca.CategoryId).Distinct().ToList();
        var attributeIds = categoryAttributes.Select(ca => ca.AttributeId).Distinct().ToList();
        
        var allWithRelations = new List<CategoryAttribute>();
        foreach (var categoryId in categoryIds)
        {
            var withRelations = await _categoryAttributeRepository.GetByCategoryIdWithRelationsAsync(categoryId, cancellationToken);
            allWithRelations.AddRange(withRelations);
        }

        return allWithRelations.Select(MapToDto).DistinctBy(ca => ca.Id);
    }

    public async Task<IEnumerable<CategoryAttributeDto>> GetCategoryAttributesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var categoryAttributes = await _categoryAttributeRepository.GetByCategoryIdWithRelationsAsync(categoryId, cancellationToken);
        return categoryAttributes.Select(MapToDto);
    }

    public async Task<IEnumerable<CategoryAttributeDto>> GetCategoryAttributesByAttributeIdAsync(int attributeId, CancellationToken cancellationToken = default)
    {
        var categoryAttributes = await _categoryAttributeRepository.GetByAttributeIdWithRelationsAsync(attributeId, cancellationToken);
        return categoryAttributes.Select(MapToDto);
    }

    public async Task<CategoryAttributeDto?> GetCategoryAttributeByCategoryAndAttributeAsync(int categoryId, int attributeId, CancellationToken cancellationToken = default)
    {
        var categoryAttribute = await _categoryAttributeRepository.GetByCategoryAndAttributeAsync(categoryId, attributeId, cancellationToken);
        return categoryAttribute == null ? null : MapToDto(categoryAttribute);
    }

    public async Task<CategoryAttributeDto> CreateCategoryAttributeAsync(CategoryAttributeDto categoryAttributeDto, CancellationToken cancellationToken = default)
    {
        // Check if already exists
        var existing = await _categoryAttributeRepository.GetByCategoryAndAttributeAsync(
            categoryAttributeDto.CategoryId, 
            categoryAttributeDto.AttributeId, 
            cancellationToken);
        
        if (existing != null)
            throw new InvalidOperationException($"CategoryAttribute with CategoryId {categoryAttributeDto.CategoryId} and AttributeId {categoryAttributeDto.AttributeId} already exists.");

        var categoryAttribute = MapToEntity(categoryAttributeDto);
        categoryAttribute = await _categoryAttributeRepository.AddAsync(categoryAttribute, cancellationToken);
        
        // Reload with relations
        var withRelations = await _categoryAttributeRepository.GetByCategoryAndAttributeAsync(
            categoryAttribute.CategoryId, 
            categoryAttribute.AttributeId, 
            cancellationToken);
        
        return MapToDto(withRelations!);
    }

    public async Task<CategoryAttributeDto> UpdateCategoryAttributeAsync(int id, CategoryAttributeDto categoryAttributeDto, CancellationToken cancellationToken = default)
    {
        var existingCategoryAttribute = await _categoryAttributeRepository.GetByIdAsync(id, cancellationToken);
        if (existingCategoryAttribute == null)
            throw new KeyNotFoundException($"CategoryAttribute with id {id} not found.");

        // Check if updating would create a duplicate
        if (existingCategoryAttribute.CategoryId != categoryAttributeDto.CategoryId || 
            existingCategoryAttribute.AttributeId != categoryAttributeDto.AttributeId)
        {
            var duplicate = await _categoryAttributeRepository.GetByCategoryAndAttributeAsync(
                categoryAttributeDto.CategoryId, 
                categoryAttributeDto.AttributeId, 
                cancellationToken);
            
            if (duplicate != null && duplicate.Id != id)
                throw new InvalidOperationException($"CategoryAttribute with CategoryId {categoryAttributeDto.CategoryId} and AttributeId {categoryAttributeDto.AttributeId} already exists.");
        }

        existingCategoryAttribute.CategoryId = categoryAttributeDto.CategoryId;
        existingCategoryAttribute.AttributeId = categoryAttributeDto.AttributeId;
        existingCategoryAttribute.IsRequired = categoryAttributeDto.IsRequired;
        existingCategoryAttribute.ModifiedOn = DateTime.UtcNow;

        await _categoryAttributeRepository.UpdateAsync(existingCategoryAttribute, cancellationToken);
        
        // Reload with relations
        var withRelations = await _categoryAttributeRepository.GetByCategoryAndAttributeAsync(
            existingCategoryAttribute.CategoryId, 
            existingCategoryAttribute.AttributeId, 
            cancellationToken);
        
        return MapToDto(withRelations!);
    }

    public async Task<bool> DeleteCategoryAttributeAsync(int id, CancellationToken cancellationToken = default)
    {
        var categoryAttribute = await _categoryAttributeRepository.GetByIdAsync(id, cancellationToken);
        if (categoryAttribute == null)
            return false;

        await _categoryAttributeRepository.DeleteAsync(categoryAttribute, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteCategoryAttributeByCategoryAndAttributeAsync(int categoryId, int attributeId, CancellationToken cancellationToken = default)
    {
        var categoryAttribute = await _categoryAttributeRepository.GetByCategoryAndAttributeAsync(categoryId, attributeId, cancellationToken);
        if (categoryAttribute == null)
            return false;

        await _categoryAttributeRepository.DeleteAsync(categoryAttribute, cancellationToken);
        return true;
    }

    private static CategoryAttributeDto MapToDto(CategoryAttribute categoryAttribute)
    {
        return new CategoryAttributeDto
        {
            Id = categoryAttribute.Id,
            CategoryId = categoryAttribute.CategoryId,
            Category = categoryAttribute.Category != null ? new CategoryDto
            {
                Id = categoryAttribute.Category.Id,
                Name = categoryAttribute.Category.Name,
                Description = categoryAttribute.Category.Description,
                IsActive = categoryAttribute.Category.IsActive,
                CreatedOn = categoryAttribute.Category.CreatedOn,
                ModifiedOn = categoryAttribute.Category.ModifiedOn
            } : null,
            AttributeId = categoryAttribute.AttributeId,
            Attribute = categoryAttribute.Attribute != null ? new AttributeDto
            {
                Id = categoryAttribute.Attribute.Id,
                AttributeKey = categoryAttribute.Attribute.AttributeKey,
                Label = categoryAttribute.Attribute.Label,
                AttributeType = categoryAttribute.Attribute.AttributeType,
                Unit = categoryAttribute.Attribute.Unit,
                CreatedOn = categoryAttribute.Attribute.CreatedOn,
                ModifiedOn = categoryAttribute.Attribute.ModifiedOn
            } : null,
            IsRequired = categoryAttribute.IsRequired,
            CreatedOn = categoryAttribute.CreatedOn,
            ModifiedOn = categoryAttribute.ModifiedOn
        };
    }

    private static CategoryAttribute MapToEntity(CategoryAttributeDto categoryAttributeDto)
    {
        return new CategoryAttribute
        {
            Id = categoryAttributeDto.Id,
            CategoryId = categoryAttributeDto.CategoryId,
            AttributeId = categoryAttributeDto.AttributeId,
            IsRequired = categoryAttributeDto.IsRequired
        };
    }
}

