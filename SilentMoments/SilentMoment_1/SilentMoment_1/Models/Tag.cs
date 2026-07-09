using System;
using System.Collections.Generic;

namespace SilentMoment_1.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<QuietMoment> Moments { get; set; } = new List<QuietMoment>();
}
