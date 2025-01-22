using System.ComponentModel.DataAnnotations.Schema;

namespace WebPortalDomain.Dtos;

public class NetworkElementDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool HasChildren { get; set; }
    
    public int NetworkElementTypeId { get; set; }
    public string NetworkElementName { get; set; }
    public int? ParentElementId { get; set; }
    [NotMapped] public List<NetworkElementDto> Children { get; set; } = new();
    [NotMapped] string TargetElementName { get; set; }
}
