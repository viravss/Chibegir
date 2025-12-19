namespace Chibegir.Application.DTOs;

public class ProductLogDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int? SourceId { get; set; }
    public string Action { get; set; } = string.Empty;
    public ProductDto? Product { get; set; }
    public SourceDto? Source { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
