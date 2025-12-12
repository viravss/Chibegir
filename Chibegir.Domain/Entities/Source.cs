using Chibegir.Domain.Common;

namespace Chibegir.Domain.Entities;

public class Source : BaseEntityInt
{
    public string SourceName { get; set; } = string.Empty;
    public string SourceBaseAddress { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

