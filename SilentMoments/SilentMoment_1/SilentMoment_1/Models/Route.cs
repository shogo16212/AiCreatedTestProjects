using System;
using System.Collections.Generic;

namespace SilentMoment_1.Models;

public partial class Route
{
    public int RouteId { get; set; }

    public string RouteName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<RouteMoment> RouteMoments { get; set; } = new List<RouteMoment>();
}
