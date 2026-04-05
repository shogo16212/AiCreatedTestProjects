using System;
using System.Collections.Generic;

namespace ProductManagement_Api_20260404.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Stock { get; set; }

    public int Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<StockHistory> StockHistories { get; set; } = new List<StockHistory>();
}
