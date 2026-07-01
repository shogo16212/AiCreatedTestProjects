using System;
using System.Collections.Generic;

namespace EchoShelf_Api.Models;

public partial class Shelf
{
    public int ShelfId { get; set; }

    public int UserId { get; set; }

    public string ShelfName { get; set; } = null!;

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ShelfItem> ShelfItems { get; set; } = new List<ShelfItem>();

    public virtual User User { get; set; } = null!;
}
