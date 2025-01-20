﻿namespace WebApi.Domain.Entities;

public partial class Zone
{
    public int ZoneKey { get; set; }

    public int? SectorKey { get; set; }

    public string? ZoneName { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Sector? SectorKeyNavigation { get; set; }
}
