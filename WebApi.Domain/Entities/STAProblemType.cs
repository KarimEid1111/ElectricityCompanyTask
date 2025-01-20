namespace WebApi.Domain.Entities;

public partial class STAProblemType
{
    public int ProblemTypeKey { get; set; }

    public string? ProblemTypeName { get; set; }

    public virtual ICollection<CuttingDownA> CuttingDownAs { get; set; } = new List<CuttingDownA>();

    public virtual ICollection<CuttingDownB> CuttingDownBs { get; set; } = new List<CuttingDownB>();
}
