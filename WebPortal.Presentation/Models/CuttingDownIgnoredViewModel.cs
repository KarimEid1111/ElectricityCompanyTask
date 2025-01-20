namespace WebPortal.Models;

public class CuttingDownIgnoredViewModel
{
    public int Id { get; set; } // Primary Key
    public int? CuttingDownIncidentId { get; set; }
    public DateOnly? ActualCreateDate { get; set; }
    public DateOnly? SynchCreateDate { get; set; }
    public string? CableName { get; set; }
    public string? CabinName { get; set; }
    public int? CreatedUser { get; set; }
}