using Chibegir.Application.DTOs;

namespace Chibegir.Application.Interfaces;

public interface ISourceService
{
    Task<SourceDto?> GetSourceByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SourceDto>> GetAllSourcesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SourceDto>> GetActiveSourcesAsync(CancellationToken cancellationToken = default);
    Task<SourceDto> CreateSourceAsync(SourceDto sourceDto, CancellationToken cancellationToken = default);
    Task<SourceDto> UpdateSourceAsync(int id, SourceDto sourceDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteSourceAsync(int id, CancellationToken cancellationToken = default);
}

