using System.ComponentModel.DataAnnotations.Schema;
using Chibegir.Domain.Common;

namespace Chibegir.Domain.Entities;

public class ProductSource : BaseEntityInt
{
    [ForeignKey("ProductId")]
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }





    [ForeignKey("SourceId")]
    public int SourceId { get; set; }
    public virtual Source Source { get; set; }
}