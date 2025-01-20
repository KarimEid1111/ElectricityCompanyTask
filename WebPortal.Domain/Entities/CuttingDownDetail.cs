namespace WebPortalDomain.Entities;

public partial class CuttingDownDetail
{
    public int CuttingDownDetailKey { get; set; }

    public int? CuttingDownKey { get; set; }

    public int? NetworkElementKey { get; set; }

    public DateOnly? ActualCreateDate { get; set; }

    public DateOnly? ActualEndDate { get; set; }

    public int? ImpactedCustomers { get; set; }

    public virtual CuttingDownHeader? CuttingDownKeyNavigation { get; set; }

    public virtual NetworkElement? NetworkElementKeyNavigation { get; set; }
}
