using System;
using System.Collections.Generic;

namespace SilentMoment_1.Models;

public partial class Place
{
    public int PlaceId { get; set; }

    public string PlaceName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<QuietMoment> QuietMoments { get; set; } = new List<QuietMoment>();
}
