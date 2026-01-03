using Chibegir.Application.DTOs;

namespace Chibegir.Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetCategoryByIdWithAttributesAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
    Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default);
    Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto categoryDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
}

