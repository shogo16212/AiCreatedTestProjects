using System;
using System.Collections.Generic;

namespace EchoShelf_Api.Models;

public partial class VwShelfSummary
{
    public int ShelfId { get; set; }

    public string ShelfName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public int? ItemCount { get; set; }
}
