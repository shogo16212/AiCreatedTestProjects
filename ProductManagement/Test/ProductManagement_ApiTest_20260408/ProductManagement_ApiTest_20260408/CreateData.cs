using ProductManagement_ApiTest_20260408.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement_ApiTest_20260408
{
    public static class CreateData
    {

        public static PostProductData CreateProduct()
        {
            return new PostProductData {ProductName = "Product", Stock = 5, Price = 5 };
        }

        public static async Task<PostHistoryData> CreateHistory(HttpClient client, string baseUrl)
        {
            var product = await GetItem.GetProduct(client, baseUrl);
            return new PostHistoryData { ProductId = product.ProductId, ActionType = "更新", Amount = 10, Memo = "Memo" };
        }
    }
}
