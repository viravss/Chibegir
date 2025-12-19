using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;

namespace Chibegir.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IRepositoryInt<Product> _productRepository;

    public ProductService(IRepositoryInt<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        return product == null ? null : MapToDto(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
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