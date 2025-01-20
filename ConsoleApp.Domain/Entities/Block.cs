
namespace ConsoleApp.Domain.Entities;

public class Block
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int CableId { get; set; }

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    public virtual Cable Cable { get; set; }
}
