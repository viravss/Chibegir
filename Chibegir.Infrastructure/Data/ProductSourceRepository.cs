using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chibegir.Infrastructure.Data;

public class ProductSourceRepository : RepositoryInt<ProductSource>, IProductSourceRepository
{
    private readonly ApplicationDbContext _context;

    public ProductSourceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ProductSource?> GetByIdWithRelationsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductSource
            .Include(ps => ps.Product)
            .Include(ps => ps.Source)
            .FirstOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ProductSource>> GetAllWithRelationsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductSource
            .Include(ps => ps.Product)
            .Include(ps => ps.Source)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductSource>> GetByProductIdWithRelationsAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductSource
            .Where(ps => ps.ProductId == productId)
            .Include(ps => ps.Product)
            .Include(ps => ps.Source)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductSource>> GetBySourceIdWithRelationsAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductSource
            .Where(ps => ps.SourceId == sourceId)
            .Include(ps => ps.Product)
            .Include(ps => ps.Source)
            .ToListAsync(cancellationToken);
    }
}
