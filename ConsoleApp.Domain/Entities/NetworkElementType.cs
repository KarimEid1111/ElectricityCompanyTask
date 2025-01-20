namespace ConsoleApp.Domain.Entities;

public class NetworkElementType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentNetworkElementId { get; set; }

    public int NetworkElementHierarchyPathId { get; set; }

    public virtual ICollection<NetworkElementType> InverseParentNetworkElement { get; set; } = new List<NetworkElementType>();

    public virtual NetworkElementHierarchyPath NetworkElementHierarchyPath { get; set; }

    public virtual ICollection<NetworkElement> NetworkElements { get; set; } = new List<NetworkElement>();

    public virtual NetworkElementType ParentNetworkElement { get; set; }
}
