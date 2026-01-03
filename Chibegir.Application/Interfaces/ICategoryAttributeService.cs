using Chibegir.Application.DTOs;

namespace Chibegir.Application.Interfaces;

public interface ICategoryAttributeService
{
    Task<CategoryAttributeDto?> GetCategoryAttributeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryAttributeDto>> GetAllCategoryAttributesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryAttributeDto>> GetCategoryAttributesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryAttributeDto>> GetCategoryAttributesByAttributeIdAsync(int attributeId, CancellationToken cancellationToken = default);
    Task<CategoryAttributeDto?> GetCategoryAttributeByCategoryAndAttributeAsync(int categoryId, int attributeId, CancellationToken cancellationToken = default);
    Task<CategoryAttributeDto> CreateCategoryAttributeAsync(CategoryAttributeDto categoryAttributeDto, CancellationToken cancellationToken = default);
    Task<CategoryAttributeDto> UpdateCategoryAttributeAsync(int id, CategoryAttributeDto categoryAttributeDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoryAttributeAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoryAttributeByCategoryAndAttributeAsync(int categoryId, int attributeId, CancellationToken cancellationToken = default);
}

