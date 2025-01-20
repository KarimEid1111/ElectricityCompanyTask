namespace ConsoleApp.Domain.Entities;

public class NetworkElementHierarchyPath
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Abbreviation { get; set; }

    public virtual ICollection<NetworkElementType> NetworkElementTypes { get; set; } = new List<NetworkElementType>();
}
