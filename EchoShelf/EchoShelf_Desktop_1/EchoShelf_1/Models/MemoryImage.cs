using System;
using System.Collections.Generic;

namespace EchoShelf_1.Models;

public partial class MemoryImage
{
    public int ImageId { get; set; }

    public int MemoryId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public long? FileSize { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Memory Memory { get; set; } = null!;
}
