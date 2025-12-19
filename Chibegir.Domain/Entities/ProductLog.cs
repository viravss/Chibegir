using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chibegir.Domain.Common;

namespace Chibegir.Domain.Entities;

public class ProductLog : BaseEntityInt
{
    [ForeignKey("ProductId")]
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

    [ForeignKey("SourceId")]
    public int? SourceId { get; set; }
    public virtual Source? Source { get; set; }

    [MaxLength(50)]
    public string Action { get; set; } = string.Empty; // "Insert" or "Update"
}
