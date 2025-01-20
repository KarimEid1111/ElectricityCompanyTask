namespace ConsoleApp.Domain.Entities;

public class User
{
    public int UserId  { get; set; }

    public string Username  { get; set; }

    public string Password { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }

    public virtual ICollection<CuttingDownHeader> CuttingDownHeaderCreatedSystemUsers { get; set; } = new List<CuttingDownHeader>();

    public virtual ICollection<CuttingDownHeader> CuttingDownHeaderUpdatedSystemUsers { get; set; } = new List<CuttingDownHeader>();

    public virtual ICollection<CuttingDownIgnored> CuttingDownIgnoreds { get; set; } = new List<CuttingDownIgnored>();
    
}
