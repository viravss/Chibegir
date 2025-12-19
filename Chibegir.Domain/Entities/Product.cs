using System.ComponentModel.DataAnnotations;
using Chibegir.Domain.Common;

namespace Chibegir.Domain.Entities;

public class Product : BaseEntityInt
{
    [MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string ProductUrl { get; set; } = string.Empty;

    //public int SourceId { get; set; }

    public string Html { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    //// Navigation property
    //public Source? Source { get; set; }
}

