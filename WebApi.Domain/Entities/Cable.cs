namespace WebApi.Domain.Entities;

public partial class Cable
{
    public int CableKey { get; set; }

    public int? CabinKey { get; set; }

    public string? CableName { get; set; }

    public virtual ICollection<Block> Blocks { get; set; } = new List<Block>();

    public virtual Cabin? CabinKeyNavigation { get; set; }
}
