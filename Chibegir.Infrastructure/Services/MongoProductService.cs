using Chibegir.Application.DTOs.NoSqlSchema;
using Chibegir.Application.Enums;
using Chibegir.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Chibegir.Infrastructure.Services;

public class MongoProductService : IMongoProductService
{
    private readonly IMongoCollection<ProductSchema> _productsCollection;

    public MongoProductService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDbConnection");
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(configuration["MongoDb:DatabaseName"] ?? "ChibegirDb");
        _productsCollection = database.GetCollection<ProductSchema>("Products");
    }

    public async Task<string> InsertProductAsync(ProductSchema product, CancellationToken cancellationToken = default)
    {
        await _productsCollection.InsertOneAsync(product, cancellationToken: cancellationToken);
        return product.Id.ToString();
    }

    public async Task<ProductSchema?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductSchema>.Filter.Eq(p => p.Id, id);
        return await _productsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductSchema?> GetProductByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductSchema>.Filter.Eq(p => p.Title, title);
        return await _productsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductSchema>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        return await _productsCollection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductSchema>> SearchProductsByTitleAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductSchema>.Filter.Regex(p => p.Title, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"));
        return await _productsCollection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductSchema>> GetProductsByBrandAsync(string brandName, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductSchema>.Filter.Eq(p => p.BrandName, brandName);
        return await _productsCollection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductSchema>> GetProductsByCategoryAsync(ProductCategoryEnum category, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductSchema>.Filter.Eq(p => p.ProductCategory, category);
        return await _productsCollection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductSchema>> GetAvailableProductsAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductSchema>.Filter.Eq(p => p.IsAvailable, true);
        return await _productsCollection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateProductAsync(int id, ProductSchema product, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductSchema>.Filter.Eq(p => p.Id, id);
        var result = await _productsCollection.ReplaceOneAsync(filter, product, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductSchema>.Filter.Eq(p => p.Id, id);
        var result = await _productsCollection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }
}

