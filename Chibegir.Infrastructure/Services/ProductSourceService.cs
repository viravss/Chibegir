using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;

namespace Chibegir.Infrastructure.Services;

public class ProductSourceService : IProductSourceService
{
    private readonly IRepositoryInt<ProductSource> _productSourceRepository;

    public ProductSourceService(IRepositoryInt<ProductSource> productSourceRepository)
    {
        _productSourceRepository = productSourceRepository;
    }

    public async Task<ProductSourceDto?> GetProductSourceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var productSource = await _productSourceRepository.GetByIdAsync(id, cancellationToken);
        return productSource == null ? null : MapToDto(productSource);
    }

    public async Task<IEnumerable<ProductSourceDto>> GetAllProductSourcesAsync(CancellationToken cancellationToken = default)
    {
        var productSources = await _productSourceRepository.GetAllAsync(cancellationToken);
        return productSources.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductSourceDto>> GetProductSourcesByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var productSources = await _productSourceRepository.FindAsync(ps => ps.ProductId == productId, cancellationToken);
        return productSources.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductSourceDto>> GetProductSourcesBySourceIdAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        var productSources = await _productSourceRepository.FindAsync(ps => ps.SourceId == sourceId, cancellationToken);
        return productSources.Select(MapToDto);
    }

    public async Task<ProductSourceDto> CreateProductSourceAsync(ProductSourceDto productSourceDto, CancellationToken cancellationToken = default)
    {
        var productSource = MapToEntity(productSourceDto);
        productSource = await _productSourceRepository.AddAsync(productSource, cancellationToken);
        return MapToDto(productSource);
    }

    public async Task<ProductSourceDto> UpdateProductSourceAsync(int id, ProductSourceDto productSourceDto, CancellationToken cancellationToken = default)
    {
        var existingProductSource = await _productSourceRepository.GetByIdAsync(id, cancellationToken);
        if (existingProductSource == null)
            throw new KeyNotFoundException($"ProductSource with id {id} not found.");

        existingProductSource.ProductId = productSourceDto.ProductId;
        existingProductSource.SourceId = productSourceDto.SourceId;
        existingProductSource.ModifiedOn = DateTime.UtcNow;

        await _productSourceRepository.UpdateAsync(existingProductSource, cancellationToken);
        return MapToDto(existingProductSource);
    }

    public async Task<bool> DeleteProductSourceAsync(int id, CancellationToken cancellationToken = default)
    {
        var productSource = await _productSourceRepository.GetByIdAsync(id, cancellationToken);
        if (productSource == null)
            return false;

        await _productSourceRepository.DeleteAsync(productSource, cancellationToken);
        return true;
    }

    private static ProductSourceDto MapToDto(ProductSource productSource)
    {
        return new ProductSourceDto
        {
            Id = productSource.Id,
            ProductId = productSource.ProductId,
            SourceId = productSource.SourceId,
            CreatedOn = productSource.CreatedOn,
            ModifiedOn = productSource.ModifiedOn
        };
    }

    private static ProductSource MapToEntity(ProductSourceDto productSourceDto)
    {
        return new ProductSource
        {
            Id = productSourceDto.Id,
            ProductId = productSourceDto.ProductId,
            SourceId = productSourceDto.SourceId
        };
    }
}
