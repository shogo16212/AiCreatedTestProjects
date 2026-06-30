using System;
using System.Collections.Generic;

namespace EchoShelf_1.Models;

public partial class EchoAnalysisDetail
{
    public int DetailId { get; set; }

    public int AnalysisId { get; set; }

    public int? TagId { get; set; }

    public int? CategoryId { get; set; }

    public decimal Score { get; set; }

    public string? Comment { get; set; }

    public virtual EchoAnalysis Analysis { get; set; } = null!;

    public virtual Category? Category { get; set; }

    public virtual Tag? Tag { get; set; }
}
