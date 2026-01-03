using Chibegir.Domain.Entities;

namespace Chibegir.Application.Interfaces;

public interface IProductExtractorService
{
    Task<string> ExtractProductWithAiAsync(int productId);
}