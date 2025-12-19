using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;

namespace Chibegir.Infrastructure.Services;

public class SourceService : ISourceService
{
    private readonly IRepositoryInt<Source> _sourceRepository;
    private readonly IProductSourceRepository _productSourceRepository;

    public SourceService(IRepositoryInt<Source> sourceRepository, IProductSourceRepository productSourceRepository)
    {
        _sourceRepository = sourceRepository;
        _productSourceRepository = productSourceRepository;
    }

    public async Task<SourceDto?> GetSourceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var source = await _sourceRepository.GetByIdAsync(id, cancellationToken);
        return source == null ? null : MapToDto(source);
    }

    public async Task<IEnumerable<SourceDto>> GetAllSourcesAsync(CancellationToken cancellationToken = default)
    {
        var sources = await _sourceRepository.GetAllAsync(cancellationToken);
        return sources.Select(MapToDto);
    }

    public async Task<IEnumerable<SourceDto>> GetActiveSourcesAsync(CancellationToken cancellationToken = default)
    {
        var sources = await _sourceRepository.FindAsync(s => s.IsActive, cancellationToken);
        return sources.Select(MapToDto);
    }

    public async Task<SourceDto> CreateSourceAsync(SourceDto sourceDto, CancellationToken cancellationToken = default)
    {
        var source = MapToEntity(sourceDto);
        source = await _sourceRepository.AddAsync(source, cancellationToken);

        //// Create ProductSource records if ProductIds are provided
        //if (sourceDto.ProductIds != null && sourceDto.ProductIds.Any())
        //{
        //    foreach (var productId in sourceDto.ProductIds)
        //    {
        //        var productSource = new ProductSource
        //        {
        //            ProductId = productId,
        //            SourceId = source.Id
        //        };
        //        await _productSourceRepository.AddAsync(productSource, cancellationToken);
        //    }
        //}

        return MapToDto(source);
    }

    public async Task<SourceDto> UpdateSourceAsync(int id, SourceDto sourceDto, CancellationToken cancellationToken = default)
    {
        var existingSource = await _sourceRepository.GetByIdAsync(id, cancellationToken);
        if (existingSource == null)
            throw new KeyNotFoundException($"Source with id {id} not found.");

        existingSource.SourceName = sourceDto.SourceName;
        existingSource.SourceBaseAddress = sourceDto.SourceBaseAddress;
        existingSource.IsActive = sourceDto.IsActive;
        existingSource.ModifiedOn = DateTime.UtcNow;

        await _sourceRepository.UpdateAsync(existingSource, cancellationToken);
        return MapToDto(existingSource);
    }

    public async Task<bool> DeleteSourceAsync(int id, CancellationToken cancellationToken = default)
    {
        var source = await _sourceRepository.GetByIdAsync(id, cancellationToken);
        if (source == null)
            return false;

        await _sourceRepository.DeleteAsync(source, cancellationToken);
        return true;
    }

    private static SourceDto MapToDto(Source source)
    {
        return new SourceDto
        {
            Id = source.Id,
            SourceName = source.SourceName,
            SourceBaseAddress = source.SourceBaseAddress,
            IsActive = source.IsActive,
            CreatedOn = source.CreatedOn,
            ModifiedOn = source.ModifiedOn
        };
    }

    private static Source MapToEntity(SourceDto sourceDto)
    {
        return new Source
        {
            Id = sourceDto.Id,
            SourceName = sourceDto.SourceName,
            SourceBaseAddress = sourceDto.SourceBaseAddress,
            IsActive = sourceDto.IsActive
        };
    }
}

