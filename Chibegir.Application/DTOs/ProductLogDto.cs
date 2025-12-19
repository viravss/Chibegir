namespace Chibegir.Application.DTOs;

public class ProductLogDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int? SourceId { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
