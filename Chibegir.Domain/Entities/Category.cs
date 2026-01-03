using System.ComponentModel.DataAnnotations;
using Chibegir.Domain.Common;

namespace Chibegir.Domain.Entities;

public class Category : BaseEntityInt
{
    [MaxLength(250)] public string Name { get; set; }

    [MaxLength(1000)] public string Description { get; set; }

    public bool IsActive { get; set; }

    public virtual List<Product> Products { get; set; }
}