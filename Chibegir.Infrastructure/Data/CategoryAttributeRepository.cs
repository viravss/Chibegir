using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chibegir.Infrastructure.Data;

public class CategoryAttributeRepository : RepositoryInt<CategoryAttribute>, ICategoryAttributeRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryAttributeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryAttribute>> GetByCategoryIdWithRelationsAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.CategoryAttribute
            .Include(ca => ca.Category)
            .Include(ca => ca.Attribute)
            .Where(ca => ca.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CategoryAttribute>> GetByAttributeIdWithRelationsAsync(int attributeId, CancellationToken cancellationToken = default)
    {
        return await _context.CategoryAttribute
            .Include(ca => ca.Category)
            .Include(ca => ca.Attribute)
            .Where(ca => ca.AttributeId == attributeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryAttribute?> GetByCategoryAndAttributeAsync(int categoryId, int attributeId, CancellationToken cancellationToken = default)
    {
        return await _context.CategoryAttribute
            .Include(ca => ca.Category)
            .Include(ca => ca.Attribute)
            .FirstOrDefaultAsync(ca => ca.CategoryId == categoryId && ca.AttributeId == attributeId, cancellationToken);
    }
}

