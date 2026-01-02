using System.ComponentModel.DataAnnotations;
using Chibegir.Domain.Common;

namespace Chibegir.Domain.Entities;

public class Attribute : BaseEntityInt
{
    [MaxLength(200)]
    public string AttributeKey { get; set; }
    [MaxLength(200)]
    public string Label { get; set; }
    [MaxLength(100)]
    public string AttributeType { get; set; }

    [MaxLength(100)]
    public string Unit { get; set; }
}