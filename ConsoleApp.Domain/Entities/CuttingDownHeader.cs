

namespace ConsoleApp.Domain.Entities;

public class CuttingDownHeader
{
    public int Id { get; set; }

    public DateOnly ActualCreateDate { get; set; }

    public DateOnly SynchCreateDate { get; set; }

    public DateOnly? SynchUpdateDate { get; set; }

    public DateOnly? ActualEndDate { get; set; }

    public bool IsPlanned { get; set; }

    public bool IsGlobal { get; set; }

    public bool IsActive { get; set; }

    public DateTime? PlannedStartDateyime { get; set; }

    public DateTime? PlannedEndDatetime { get; set; }

    public int? CreatedSystemUserId { get; set; }

    public int? UpdatedSystemUserId { get; set; }

    public int CuttingDownProblemId { get; set; }

    public int ChannelId { get; set; }

    public int? CuttingDownIncidentId { get; set; }

    public virtual Channel Channel { get; set; }

    public virtual User CreatedSystemUser { get; set; }

    public virtual ICollection<CuttingDownDetail> CuttingDownDetails { get; set; } = new List<CuttingDownDetail>();

    public virtual FtaProblemType CuttingDownFtaProblem { get; set; }

    public virtual User UpdatedSystemUser { get; set; }
}
