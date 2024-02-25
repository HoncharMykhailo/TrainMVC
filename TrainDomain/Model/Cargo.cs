using System;
using System.Collections.Generic;

namespace TrainDomain.Model;

public partial class Cargo
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int Weight { get; set; }

    public int Volume { get; set; }

    public string Contain { get; set; } = null!;

    public int StationId { get; set; }

    public virtual ICollection<Carriage> Carriages { get; set; } = new List<Carriage>();

    public virtual Client Client { get; set; } = null!;

    public virtual Station Station { get; set; } = null!;
}
