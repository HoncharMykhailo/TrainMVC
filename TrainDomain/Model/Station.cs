using System;
using System.Collections.Generic;

namespace TrainDomain.Model;

public partial class Station
{
    public int Id { get; set; }

    public int CityId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Train> Trains { get; set; } = new List<Train>();
}
