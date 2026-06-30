using System;
using System.Collections.Generic;

namespace EchoShelf_1.Models;

public partial class ShelfItem
{
    public int ShelfItemId { get; set; }

    public int ShelfId { get; set; }

    public int MemoryId { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Memory Memory { get; set; } = null!;

    public virtual Shelf Shelf { get; set; } = null!;
}
