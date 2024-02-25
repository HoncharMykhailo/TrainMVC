using System;
using System.Collections.Generic;

namespace TrainDomain.Model;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();
}
