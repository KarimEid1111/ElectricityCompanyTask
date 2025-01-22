namespace ConsoleApp.Domain.Entities;

public partial class Flat
{
    public int FlatKey { get; set; }

    public int? BuildingKey { get; set; }
    
    public string? FlatName { get; set; }

    public virtual Building? BuildingKeyNavigation { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
