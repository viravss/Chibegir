namespace Chibegir.Application.DTOs;

public class ProductSourceDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int SourceId { get; set; }
    public ProductDto? Product { get; set; }
    public SourceDto? Source { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
