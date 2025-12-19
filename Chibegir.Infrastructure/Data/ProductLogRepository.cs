using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chibegir.Infrastructure.Data;

public class ProductLogRepository : RepositoryInt<ProductLog>, IProductLogRepository
{
    private readonly ApplicationDbContext _context;

    public ProductLogRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ProductLog?> GetByIdWithRelationsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductLog
            .Include(pl => pl.Product)
            .Include(pl => pl.Source)
            .FirstOrDefaultAsync(pl => pl.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ProductLog>> GetAllWithRelationsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductLog
            .Include(pl => pl.Product)
            .Include(pl => pl.Source)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductLog>> GetByProductIdWithRelationsAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductLog
            .Where(pl => pl.ProductId == productId)
            .Include(pl => pl.Product)
            .Include(pl => pl.Source)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductLog>> GetBySourceIdWithRelationsAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductLog
            .Where(pl => pl.SourceId == sourceId)
            .Include(pl => pl.Product)
            .Include(pl => pl.Source)
            .ToListAsync(cancellationToken);
    }
}
