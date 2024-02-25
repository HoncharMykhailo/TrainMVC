using System;
using System.Collections.Generic;

namespace TrainDomain.Model;

public partial class Driver
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int TrainId { get; set; }

    public virtual Train Train { get; set; } = null!;
}
