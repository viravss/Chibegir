namespace Chibegir.Domain.Common;

public abstract class BaseEntityInt
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedOn { get; set; }
}

