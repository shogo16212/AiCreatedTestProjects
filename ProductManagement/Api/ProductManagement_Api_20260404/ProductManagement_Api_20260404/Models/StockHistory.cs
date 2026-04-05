using System;
using System.Collections.Generic;

namespace ProductManagement_Api_20260404.Models;

public partial class StockHistory
{
    public int HistoryId { get; set; }

    public int ProductId { get; set; }

    public string ActionType { get; set; } = null!;

    public int? Amount { get; set; }

    public string? Memo { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
