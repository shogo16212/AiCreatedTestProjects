using System;
using System.Collections.Generic;

namespace EchoShelf_1.Models;

public partial class VwMemorySummary
{
    public int MemoryId { get; set; }

    public string UserName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateOnly MemoryDate { get; set; }

    public bool IsFavorite { get; set; }
}
