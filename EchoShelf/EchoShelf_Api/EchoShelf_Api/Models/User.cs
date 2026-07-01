using System;
using System.Collections.Generic;

namespace EchoShelf_Api.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? AvatarImagePath { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<EchoAnalysis> EchoAnalyses { get; set; } = new List<EchoAnalysis>();

    public virtual ICollection<Memory> Memories { get; set; } = new List<Memory>();

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}
