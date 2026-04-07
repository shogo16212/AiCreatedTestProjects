using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductManagement_ApiTest_20260408
{
    public static class Common
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };

        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, options);
        }
        public static T FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}
