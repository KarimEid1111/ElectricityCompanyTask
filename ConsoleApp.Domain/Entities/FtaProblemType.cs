namespace ConsoleApp.Domain.Entities;

public class FtaProblemType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<CuttingDownHeader> CuttingDownHeaders { get; set; } = new List<CuttingDownHeader>();
}
