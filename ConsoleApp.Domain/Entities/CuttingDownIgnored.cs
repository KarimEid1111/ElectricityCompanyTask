namespace ConsoleApp.Domain.Entities;

public partial class CuttingDownIgnored
{
    public int? CuttingDownIncidentId { get; set; }

    public DateOnly? ActualCreateDate { get; set; }

    public DateOnly? SynchCreateDate { get; set; }

    public string? CableName { get; set; }

    public string? CabinName { get; set; }

    public int? CreatedUser { get; set; }

    public virtual User? CreatedUserNavigation { get; set; }
}
