namespace WebPortalDomain.Dtos;

using System.ComponentModel.DataAnnotations.Schema;

public class GetHierarchyPathDto
{
    [Column("Network_Element_Key")] public int NetworkElementKey { get; set; }
    [Column("Network_Element_Name")] public string NetworkElementName { get; set; }
    [Column("Network_Element_Type_Key")] public int NetworkElementTypeKey { get; set; }
    [Column("Parent_Network_Element_Key")] public int? ParentNetworkElementKey { get; set; }
}