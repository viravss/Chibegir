namespace Chibegir.Application.DTOs;

public class AttributeDto
{
    public int Id { get; set; }
    public string AttributeKey { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string AttributeType { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

