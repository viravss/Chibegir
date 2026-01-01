using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chibegir.Infrastructure.Data;

public class ProductRepository : RepositoryInt<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdWithSourceAsync(int id, CancellationToken cancellationToken = default)
    {
        // Get product and manually load related ProductSource with Source and Category
        var product = await _context.Product
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (product != null)
        {
            // Explicitly load ProductSource with Source for this product
            var productSources = await _context.ProductSource
                .Where(ps => ps.ProductId == product.Id)
                .Include(ps => ps.Source)
                .ToListAsync(cancellationToken);
        }

        return product;
    }

    public async Task<IEnumerable<Product>> GetAllWithSourcesAsync(CancellationToken cancellationToken = default)
    {
        // Get all products with Category
        var products = await _context.Product
            .Include(p => p.Category)
            .ToListAsync(cancellationToken);

        // Optionally load ProductSource for all products
        var productIds = products.Select(p => p.Id).ToList();
        var productSources = await _context.ProductSource
            .Where(ps => productIds.Contains(ps.ProductId))
            .Include(ps => ps.Source)
            .ToListAsync(cancellationToken);

        return products;
    }

    public async Task<IEnumerable<Product>> GetAvailableWithSourcesAsync(CancellationToken cancellationToken = default)
    {
        // Get available products with Category
        var products = await _context.Product
            .Include(p => p.Category)
            .Where(p => p.IsAvailable)
            .ToListAsync(cancellationToken);

        // Optionally load ProductSource for available products
        var productIds = products.Select(p => p.Id).ToList();
        var productSources = await _context.ProductSource
            .Where(ps => productIds.Contains(ps.ProductId))
            .Include(ps => ps.Source)
            .ToListAsync(cancellationToken);

        return products;
    }
}
