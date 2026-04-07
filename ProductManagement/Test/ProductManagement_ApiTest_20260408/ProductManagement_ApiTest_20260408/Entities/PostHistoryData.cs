using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement_ApiTest_20260408.Entities
{
    public class PostHistoryData
    {
        public int? ProductId { get; set; }
        public string? ActionType { get; set; }
        public int? Amount { get; set; }
        public string? Memo { get; set; }
    }
}
