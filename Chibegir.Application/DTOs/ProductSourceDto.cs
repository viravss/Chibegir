namespace Chibegir.Application.DTOs;

public class ProductSourceDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int SourceId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
