

namespace ConsoleApp.Domain.Entities;

public class CuttingDownA
{
    public int Id { get; set; }

    public string CuttingDownCabinName { get; set; }

    public DateOnly CreateDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsPlanned { get; set; }

    public bool IsGlobal { get; set; }

    public bool IsActive { get; set; }

    public DateTime? PlannedStartDatetime { get; set; }

    public DateTime? PlannedEndDatetime { get; set; }

    public string CreatedUserName { get; set; }

    public string UpdatedUserName { get; set; }

    public int ProblemTypeId { get; set; }

    public virtual StaProblemType StaProblemType { get; set; }
}
