namespace ConsoleApp.Domain.Entities;

public class StaProblemType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<CuttingDownA> CuttingDownAs { get; set; } = new List<CuttingDownA>();

    public virtual ICollection<CuttingDownB> CuttingDownBs { get; set; } = new List<CuttingDownB>();
}
