﻿namespace WebPortalDomain.Entities;

public partial class City
{
    public int CityKey { get; set; }

    public int? ZoneKey { get; set; }

    public string? CityName { get; set; }

    public virtual ICollection<Station> Stations { get; set; } = new List<Station>();

    public virtual Zone? ZoneKeyNavigation { get; set; }
}
