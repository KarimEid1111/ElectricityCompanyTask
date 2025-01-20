

namespace ConsoleApp.Domain.Entities;

public class CuttingDownIgnored
{
    public int Id { get; set; }

    public DateOnly ActualCreateDate { get; set; }

    public DateOnly? SynchDate { get; set; }

    public string CableName { get; set; }

    public string CabinName { get; set; }

    public int? CreatedUserId { get; set; }

    public virtual User CreatedUser { get; set; }
}
