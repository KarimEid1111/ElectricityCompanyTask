namespace WebApi.Domain.Entities;

public partial class Block
{
    public int BlockKey { get; set; }

    public int? CableKey { get; set; }

    public string? BlockName { get; set; }

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    public virtual Cable? CableKeyNavigation { get; set; }
}
