namespace WebPortalDomain.Entities;

public partial class FTAProblemType
{
    public int ProblemTypeKey { get; set; }

    public string? ProblemTypeName { get; set; }

    public virtual ICollection<CuttingDownHeader> CuttingDownHeaders { get; set; } = new List<CuttingDownHeader>();
}
