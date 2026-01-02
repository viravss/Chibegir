using System.ComponentModel.DataAnnotations.Schema;
using Chibegir.Domain.Common;

namespace Chibegir.Domain.Entities;

public class CategoryAttribute : BaseEntityInt
{
    [ForeignKey("CategoryId")]
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }


    [ForeignKey("AttributeId")]
    public int AttributeId { get; set; }
    public virtual Attribute Attribute { get; set; }


    public bool IsRequired { get; set; }
}