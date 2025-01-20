namespace ConsoleApp.Domain.Entities;

public class Subscribtion
{
    public int Id { get; set; }

    public int BuildingId { get; set; }

    public int? FlatId { get; set; }

    public int? PaletId { get; set; }

    public int? MeterId { get; set; }

    public virtual Building Building { get; set; }

    public virtual Flat Flat { get; set; }
}
