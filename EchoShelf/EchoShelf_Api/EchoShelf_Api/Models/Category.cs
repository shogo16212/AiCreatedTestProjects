using System;
using System.Collections.Generic;

namespace EchoShelf_Api.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<EchoAnalysisDetail> EchoAnalysisDetails { get; set; } = new List<EchoAnalysisDetail>();

    public virtual ICollection<Memory> Memories { get; set; } = new List<Memory>();
}
