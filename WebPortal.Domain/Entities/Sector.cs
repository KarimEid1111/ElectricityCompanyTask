namespace WebPortalDomain.Entities;

public partial class Sector
{
    public int SectorKey { get; set; }

    public int? GovernrateKey { get; set; }

    public string? SectorName { get; set; }

    public virtual Governorate? GovernrateKeyNavigation { get; set; }

    public virtual ICollection<Zone> Zones { get; set; } = new List<Zone>();
}
