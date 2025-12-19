using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;

namespace Chibegir.Infrastructure.Services;

public class ProductLogService : IProductLogService
{
    private readonly IRepositoryInt<ProductLog> _productLogRepository;

    public ProductLogService(IRepositoryInt<ProductLog> productLogRepository)
    {
        _productLogRepository = productLogRepository;
    }

    public async Task<ProductLogDto?> GetProductLogByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var productLog = await _productLogRepository.GetByIdAsync(id, cancellationToken);
        return productLog == null ? null : MapToDto(productLog);
    }

    public async Task<IEnumerable<ProductLogDto>> GetAllProductLogsAsync(CancellationToken cancellationToken = default)
    {
        var productLogs = await _productLogRepository.GetAllAsync(cancellationToken);
        return productLogs.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductLogDto>> GetProductLogsByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var productLogs = await _productLogRepository.FindAsync(pl => pl.ProductId == productId, cancellationToken);
        return productLogs.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductLogDto>> GetProductLogsBySourceIdAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        var productLogs = await _productLogRepository.FindAsync(pl => pl.SourceId == sourceId, cancellationToken);
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
        return new ProductLogDto
        {
            Id = productLog.Id,
            ProductId = productLog.ProductId,
            SourceId = productLog.SourceId,
            Action = productLog.Action,
            CreatedOn = productLog.CreatedOn,
            ModifiedOn = productLog.ModifiedOn
        };
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
