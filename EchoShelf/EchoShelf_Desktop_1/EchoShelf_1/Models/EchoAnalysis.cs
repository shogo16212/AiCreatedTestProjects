using System;
using System.Collections.Generic;

namespace EchoShelf_1.Models;

public partial class EchoAnalysis
{
    public int AnalysisId { get; set; }

    public int UserId { get; set; }

    public DateTime AnalysisDate { get; set; }

    public string? Summary { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<EchoAnalysisDetail> EchoAnalysisDetails { get; set; } = new List<EchoAnalysisDetail>();

    public virtual User User { get; set; } = null!;
}
