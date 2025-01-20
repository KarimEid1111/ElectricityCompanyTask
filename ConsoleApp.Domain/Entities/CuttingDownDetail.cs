

namespace ConsoleApp.Domain.Entities;

public class CuttingDownDetail
{
    public int Id { get; set; }

    public DateOnly ActualCreateDate { get; set; }

    public DateOnly? ActualEndDate { get; set; }

    public int ImpactedCustomers { get; set; }

    public int NetworkElementId { get; set; }

    public int CuttingDownHeaderId { get; set; }

    public virtual CuttingDownHeader CuttingDownHeader { get; set; }

    public virtual NetworkElement NetworkElement { get; set; }
}
