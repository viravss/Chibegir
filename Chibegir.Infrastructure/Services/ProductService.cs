using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;

namespace Chibegir.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductSourceRepository _productSourceRepository;
    private readonly IProductLogRepository _productLogRepository;

    public ProductService(IProductRepository productRepository, IProductSourceRepository productSourceRepository, IProductLogRepository productLogRepository)
    {
        _productRepository = productRepository;
        _productSourceRepository = productSourceRepository;
        _productLogRepository = productLogRepository;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdWithSourceAsync(id, cancellationToken);
        return product == null ? null : await MapToDtoWithSourcesAsync(product, cancellationToken);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllWithSourcesAsync(cancellationToken);
        return products.Select(MapToDto);
    }

    //public async Task<IEnumerable<ProductDto>> GetProductsBySourceIdAsync(int sourceId, CancellationToken cancellationToken = default)
    //{
    //    var products = await _productRepository.FindAsync(p => p.SourceId == sourceId, cancellationToken);
    //    return products.Select(MapToDto);
    //}

    public async Task<IEnumerable<ProductDto>> GetAvailableProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.FindAsync(p => p.IsAvailable, cancellationToken);
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = MapToEntity(productDto);
        product = await _productRepository.AddAsync(product, cancellationToken);

        // Create ProductSource record if SourceId is provided
        if (productDto.SourceId.HasValue && productDto.SourceId.Value > 0)
        {
            var productSource = new ProductSource
            {
                ProductId = product.Id,
                SourceId = productDto.SourceId.Value
            };
            await _productSourceRepository.AddAsync(productSource, cancellationToken);
        }

        // Create ProductLog for Insert action
        var productLog = new ProductLog
        {
            ProductId = product.Id,
            SourceId = productDto.SourceId,
            Action = "Insert"
        };
        await _productLogRepository.AddAsync(productLog, cancellationToken);

        return MapToDto(product);
    }

    public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (existingProduct == null)
            throw new KeyNotFoundException($"Product with id {id} not found.");

        existingProduct.Title = productDto.Title;
        existingProduct.LastUpdate = productDto.LastUpdate;
        existingProduct.ProductUrl = productDto.ProductUrl;
        existingProduct.Html = productDto.Html;
        existingProduct.Price = productDto.Price;
        existingProduct.IsAvailable = productDto.IsAvailable;
        existingProduct.ModifiedOn = DateTime.UtcNow;

        await _productRepository.UpdateAsync(existingProduct, cancellationToken);

        // Get SourceId from ProductSource or use from productDto
        int? sourceId = productDto.SourceId;
        if (!sourceId.HasValue)
        {
            var productSources = await _productSourceRepository.GetByProductIdWithRelationsAsync(id, cancellationToken);
            sourceId = productSources.FirstOrDefault()?.SourceId;
        }

        // Create ProductLog for Update action
        var productLog = new ProductLog
        {
            ProductId = existingProduct.Id,
            SourceId = sourceId,
            Action = "Update"
        };
        await _productLogRepository.AddAsync(productLog, cancellationToken);

        return MapToDto(existingProduct);
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (product == null)
            return false;

        await _productRepository.DeleteAsync(product, cancellationToken);
        return true;
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Title = product.Title,
            LastUpdate = product.LastUpdate,
            ProductUrl = product.ProductUrl,
            Html = product.Html,
            Price = product.Price,
            IsAvailable = product.IsAvailable,
            CreatedOn = product.CreatedOn,
            ModifiedOn = product.ModifiedOn
        };
    }

    private async Task<ProductDto> MapToDtoWithSourcesAsync(Product product, CancellationToken cancellationToken = default)
    {
        var dto = MapToDto(product);
        
        // Get ProductSources with Sources for this product
        var productSources = await _productSourceRepository.GetByProductIdWithRelationsAsync(product.Id, cancellationToken);
        if (productSources.Any())
        {
            dto.Sources = productSources
                .Where(ps => ps.Source != null)
                .Select(ps => new SourceDto
                {
                    Id = ps.Source!.Id,
                    SourceName = ps.Source.SourceName,
                    SourceBaseAddress = ps.Source.SourceBaseAddress,
                    IsActive = ps.Source.IsActive,
                    CreatedOn = ps.Source.CreatedOn,
                    ModifiedOn = ps.Source.ModifiedOn
                })
                .ToList();
            
            // Set SourceId from first source if available
            dto.SourceId = dto.Sources.FirstOrDefault()?.Id;
        }
        
        return dto;
    }

    private static Product MapToEntity(ProductDto productDto)
    {
        return new Product
        {
            Id = productDto.Id,
            Title = productDto.Title,
            LastUpdate = productDto.LastUpdate,
            ProductUrl = productDto.ProductUrl,
            Html = productDto.Html,
            Price = productDto.Price,
            IsAvailable = productDto.IsAvailable
        };
    }
}