using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;

namespace Chibegir.Infrastructure.Services;

public class ProductLogService : IProductLogService
{
    private readonly IProductLogRepository _productLogRepository;

    public ProductLogService(IProductLogRepository productLogRepository)
    {
        _productLogRepository = productLogRepository;
    }

    public async Task<ProductLogDto?> GetProductLogByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var productLog = await _productLogRepository.GetByIdWithRelationsAsync(id, cancellationToken);
        return productLog == null ? null : MapToDto(productLog);
    }

    public async Task<IEnumerable<ProductLogDto>> GetAllProductLogsAsync(CancellationToken cancellationToken = default)
    {
        var productLogs = await _productLogRepository.GetAllWithRelationsAsync(cancellationToken);
        return productLogs.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductLogDto>> GetProductLogsByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var productLogs = await _productLogRepository.GetByProductIdWithRelationsAsync(productId, cancellationToken);
        return productLogs.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductLogDto>> GetProductLogsBySourceIdAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        var productLogs = await _productLogRepository.GetBySourceIdWithRelationsAsync(sourceId, cancellationToken);
        return productLogs.Select(MapToDto);
    }

    public async Task<ProductLogDto> CreateProductLogAsync(ProductLogDto productLogDto, CancellationToken cancellationToken = default)
    {
        var productLog = MapToEntity(productLogDto);
        productLog = await _productLogRepository.AddAsync(productLog, cancellationToken);
        return MapToDto(productLog);
    }

    private static ProductLogDto MapToDto(ProductLog productLog)
    {
        var dto = new ProductLogDto
        {
            Id = productLog.Id,
            ProductId = productLog.ProductId,
            SourceId = productLog.SourceId,
            Action = productLog.Action,
            CreatedOn = productLog.CreatedOn,
            ModifiedOn = productLog.ModifiedOn
        };

        // Map included Product if available
        if (productLog.Product != null)
        {
            dto.Product = new ProductDto
            {
                Id = productLog.Product.Id,
                Title = productLog.Product.Title,
                LastUpdate = productLog.Product.LastUpdate,
                ProductUrl = productLog.Product.ProductUrl,
                Html = productLog.Product.Html,
                Price = productLog.Product.Price,
                IsAvailable = productLog.Product.IsAvailable,
                CreatedOn = productLog.Product.CreatedOn,
                ModifiedOn = productLog.Product.ModifiedOn
            };
        }

        // Map included Source if available
        if (productLog.Source != null)
        {
            dto.Source = new SourceDto
            {
                Id = productLog.Source.Id,
                SourceName = productLog.Source.SourceName,
                SourceBaseAddress = productLog.Source.SourceBaseAddress,
                IsActive = productLog.Source.IsActive,
                CreatedOn = productLog.Source.CreatedOn,
                ModifiedOn = productLog.Source.ModifiedOn
            };
        }

        return dto;
    }

    private static ProductLog MapToEntity(ProductLogDto productLogDto)
    {
        return new ProductLog
        {
            Id = productLogDto.Id,
            ProductId = productLogDto.ProductId,
            SourceId = productLogDto.SourceId,
            Action = productLogDto.Action
        };
    }
}
