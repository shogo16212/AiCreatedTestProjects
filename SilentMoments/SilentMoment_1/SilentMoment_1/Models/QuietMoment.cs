using System;
using System.Collections.Generic;

namespace SilentMoment_1.Models;

public partial class QuietMoment
{
    public int MomentId { get; set; }

    public string Title { get; set; } = null!;

    public int QuietLevel { get; set; }

    public string? Memo { get; set; }

    public string? PhotoUrl { get; set; }

    public int PlaceId { get; set; }

    public DateTime RecordedAt { get; set; }

    public virtual Place Place { get; set; } = null!;

    public virtual ICollection<RouteMoment> RouteMoments { get; set; } = new List<RouteMoment>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
