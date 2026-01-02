namespace Chibegir.Application.DTOs;

public class CategoryAttributeDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public CategoryDto? Category { get; set; }
    public int AttributeId { get; set; }
    public AttributeDto? Attribute { get; set; }
    public bool IsRequired { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

