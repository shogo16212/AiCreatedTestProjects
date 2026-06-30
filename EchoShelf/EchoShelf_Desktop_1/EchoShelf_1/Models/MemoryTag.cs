using System;
using System.Collections.Generic;

namespace EchoShelf_1.Models;

public partial class MemoryTag
{
    public int MemoryTagId { get; set; }

    public int MemoryId { get; set; }

    public int TagId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Memory Memory { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
