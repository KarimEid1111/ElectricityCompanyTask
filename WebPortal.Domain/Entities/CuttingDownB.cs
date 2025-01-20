namespace WebPortalDomain.Entities;

public partial class CuttingDownB
{
    public int CuttingDownBIncidentId { get; set; }

    public string? CuttingDownCableName { get; set; }

    public int? ProblemTypeKey { get; set; }

    public DateOnly? CreateDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsPlanned { get; set; }

    public bool? IsGlobal { get; set; }

    public DateOnly? PlannedStartDts { get; set; }

    public DateOnly? PlannedEndDts { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedUser { get; set; }

    public int? UpdatedUser { get; set; }

    public virtual STAProblemType? ProblemTypeKeyNavigation { get; set; }
}
