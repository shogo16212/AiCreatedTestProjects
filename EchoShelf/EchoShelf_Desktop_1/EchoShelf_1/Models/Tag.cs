using System;
using System.Collections.Generic;

namespace EchoShelf_1.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<EchoAnalysisDetail> EchoAnalysisDetails { get; set; } = new List<EchoAnalysisDetail>();

    public virtual ICollection<MemoryTag> MemoryTags { get; set; } = new List<MemoryTag>();
}
