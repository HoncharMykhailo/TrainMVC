using System;
using System.Collections.Generic;

namespace TrainDomain.Model;

public partial class Carriage
{
    public int Id { get; set; }

    public int TrainId { get; set; }

    public int MaxWeight { get; set; }

    public int MaxVolume { get; set; }

    public int Number { get; set; }

    public int? CargoId { get; set; }

    public virtual Cargo? Cargo { get; set; }

    public virtual Train Train { get; set; } = null!;
}
