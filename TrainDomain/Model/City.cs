using System;
using System.Collections.Generic;

namespace TrainDomain.Model;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Station> Stations { get; set; } = new List<Station>();
}
