using System;
using System.Collections.Generic;

namespace EchoShelf_1.Models;

public partial class Memory
{
    public int MemoryId { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string Episode { get; set; } = null!;

    public DateOnly MemoryDate { get; set; }

    public bool IsFavorite { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<MemoryImage> MemoryImages { get; set; } = new List<MemoryImage>();

    public virtual ICollection<MemoryTag> MemoryTags { get; set; } = new List<MemoryTag>();

    public virtual ICollection<ShelfItem> ShelfItems { get; set; } = new List<ShelfItem>();

    public virtual User User { get; set; } = null!;
}
