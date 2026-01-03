using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;

namespace Chibegir.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepositoryInt<Category> _categoryRepository;
    private readonly ICategoryAttributeService _categoryAttributeService;

    public CategoryService(IRepositoryInt<Category> categoryRepository, ICategoryAttributeService categoryAttributeService)
    {
        _categoryRepository = categoryRepository;
        _categoryAttributeService = categoryAttributeService;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        return category == null ? null : MapToDto(category);
    }

    public async Task<CategoryDto?> GetCategoryByIdWithAttributesAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
            return null;

        var categoryDto = MapToDto(category);
        
        // Load attributes for this category
        var categoryAttributes = await _categoryAttributeService.GetCategoryAttributesByCategoryIdAsync(id, cancellationToken);
        categoryDto.Attributes = categoryAttributes.ToList();

        return categoryDto;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(MapToDto);
    }

    public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.FindAsync(c => c.IsActive, cancellationToken);
        return categories.Select(MapToDto);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        var category = MapToEntity(categoryDto);
        category = await _categoryRepository.AddAsync(category, cancellationToken);
        return MapToDto(category);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (existingCategory == null)
            throw new KeyNotFoundException($"Category with id {id} not found.");

        existingCategory.Name = categoryDto.Name;
        existingCategory.Description = categoryDto.Description;
        existingCategory.IsActive = categoryDto.IsActive;
        existingCategory.ModifiedOn = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(existingCategory, cancellationToken);
        return MapToDto(existingCategory);
    }

    public async Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
            return false;

        await _categoryRepository.DeleteAsync(category, cancellationToken);
        return true;
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedOn = category.CreatedOn,
            ModifiedOn = category.ModifiedOn
        };
    }

    private static Category MapToEntity(CategoryDto categoryDto)
    {
        return new Category
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            IsActive = categoryDto.IsActive
        };
    }
}

