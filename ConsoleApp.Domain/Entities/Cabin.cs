

namespace ConsoleApp.Domain.Entities;

public class Cabin
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int TowerId { get; set; }

    public virtual ICollection<Cable> Cables { get; set; } = new List<Cable>();

    public virtual Tower Tower { get; set; }
}
