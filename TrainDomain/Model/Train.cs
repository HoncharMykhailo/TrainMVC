using System;
using System.Collections.Generic;

namespace TrainDomain.Model;

public partial class Train
{
    public int Id { get; set; }

    public string Model { get; set; } = null!;

    public int Power { get; set; }

    public int StationId { get; set; }

    public virtual ICollection<Carriage> Carriages { get; set; } = new List<Carriage>();

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public virtual Station Station { get; set; } = null!;
}
