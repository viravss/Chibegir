using Chibegir.Application.Enums;

namespace Chibegir.Application.DTOs.NoSqlSchema;

public class ProductSchema
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ProductUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public string BrandName { get; set; }
    public ProductCategoryEnum ProductCategory { get; set; }
    public List<ProductAttributeSchema> Attributes { get; set; }
}