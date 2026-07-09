using System;
using System.Collections.Generic;

namespace SilentMoment_1.Models;

public partial class RouteMoment
{
    public int RouteId { get; set; }

    public int MomentId { get; set; }

    public int SequenceNo { get; set; }

    public virtual QuietMoment Moment { get; set; } = null!;

    public virtual Route Route { get; set; } = null!;
}
