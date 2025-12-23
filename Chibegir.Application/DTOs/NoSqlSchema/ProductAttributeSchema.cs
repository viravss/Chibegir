using System.Runtime.InteropServices.Marshalling;
using Chibegir.Application.Enums;

namespace Chibegir.Application.DTOs.NoSqlSchema;

public class ProductAttributeSchema
{
    public string Key { get; set; }
    public string Label { get; set; }
    public AttributeDataTypeEnum Type { get; set; }
    public string Unit { get; set; }
    public string Value { get; set; }
}