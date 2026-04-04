using System;
using System.Collections.Generic;

namespace ProductManagementApp_20260319.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Stock { get; set; }

    public int Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<StockHistory> StockHistories { get; set; } = new List<StockHistory>();
}
