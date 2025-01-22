namespace ConsoleApp.Domain.Entities;

public partial class User
{
    public int UserKey { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<CuttingDownHeader> CuttingDownHeaderCreateSystemUsers { get; set; } = new List<CuttingDownHeader>();

    public virtual ICollection<CuttingDownHeader> CuttingDownHeaderUpdateSystemUsers { get; set; } = new List<CuttingDownHeader>();
}
