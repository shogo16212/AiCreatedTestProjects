using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement_ApiTest_20260408
{
    public static class GetItem
    {

        public static async Task<Product> GetProduct(HttpClient client, string baseUrl)
        {
            var response = await client.GetAsync(baseUrl + "api/products");
            var json = await response.Content.ReadAsStringAsync();
            var products = json.FromJson<List<Product>>();

            return products.LastOrDefault();
        }
    }
}
