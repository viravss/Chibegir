using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;

namespace Chibegir.Infrastructure.Services;

public class ProductSourceService : IProductSourceService
{
    private readonly IProductSourceRepository _productSourceRepository;

    public ProductSourceService(IProductSourceRepository productSourceRepository)
    {
        _productSourceRepository = productSourceRepository;
    }

    public async Task<ProductSourceDto?> GetProductSourceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var productSource = await _productSourceRepository.GetByIdWithRelationsAsync(id, cancellationToken);
        return productSource == null ? null : MapToDto(productSource);
    }

    public async Task<IEnumerable<ProductSourceDto>> GetAllProductSourcesAsync(CancellationToken cancellationToken = default)
    {
        var productSources = await _productSourceRepository.GetAllWithRelationsAsync(cancellationToken);
        return productSources.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductSourceDto>> GetProductSourcesByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var productSources = await _productSourceRepository.GetByProductIdWithRelationsAsync(productId, cancellationToken);
        return productSources.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductSourceDto>> GetProductSourcesBySourceIdAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        var productSources = await _productSourceRepository.GetBySourceIdWithRelationsAsync(sourceId, cancellationToken);
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
        var dto = new ProductSourceDto
        {
            Id = productSource.Id,
            ProductId = productSource.ProductId,
            SourceId = productSource.SourceId,
            CreatedOn = productSource.CreatedOn,
            ModifiedOn = productSource.ModifiedOn
        };

        // Map included Product if available
        if (productSource.Product != null)
        {
            dto.Product = new ProductDto
            {
                Id = productSource.Product.Id,
                Title = productSource.Product.Title,
                LastUpdate = productSource.Product.LastUpdate,
                ProductUrl = productSource.Product.ProductUrl,
                Html = productSource.Product.Html,
                Price = productSource.Product.Price,
                IsAvailable = productSource.Product.IsAvailable,
                CreatedOn = productSource.Product.CreatedOn,
                ModifiedOn = productSource.Product.ModifiedOn
            };
        }

        // Map included Source if available
        if (productSource.Source != null)
        {
            dto.Source = new SourceDto
            {
                Id = productSource.Source.Id,
                SourceName = productSource.Source.SourceName,
                SourceBaseAddress = productSource.Source.SourceBaseAddress,
                IsActive = productSource.Source.IsActive,
                CreatedOn = productSource.Source.CreatedOn,
                ModifiedOn = productSource.Source.ModifiedOn
            };
        }

        return dto;
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
