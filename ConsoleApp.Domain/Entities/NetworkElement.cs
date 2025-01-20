namespace ConsoleApp.Domain.Entities;

public class NetworkElement
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentNetworkElementId { get; set; }

    public int NetworkElementTypeId { get; set; }

    public virtual ICollection<CuttingDownDetail> CuttingDownDetails { get; set; } = new List<CuttingDownDetail>();

    public virtual ICollection<NetworkElement> InverseParentNetworkElement { get; set; } = new List<NetworkElement>();

    public virtual NetworkElementType NetworkElementType { get; set; }

    public virtual NetworkElement ParentNetworkElement { get; set; }
}
