namespace Chibegir.Application.DTOs;

public class SourceDto
{
    public int Id { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public string SourceBaseAddress { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

