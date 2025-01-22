namespace ConsoleApp.Domain.Entities;

public partial class Cabin
{
    public int CabinKey { get; set; }

    public int? TowerKey { get; set; }

    public string? CabinName { get; set; }

    public virtual ICollection<Cable> Cables { get; set; } = new List<Cable>();

    public virtual Tower? TowerKeyNavigation { get; set; }
}
