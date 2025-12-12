namespace Chibegir.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime LastUpdate { get; set; }
    public string ProductUrl { get; set; } = string.Empty;
    public int SourceId { get; set; }
    public string Html { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

