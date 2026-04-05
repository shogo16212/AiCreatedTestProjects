using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductManagement_Api_20260404.Models;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace ProductManagement_Api_20260404.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private DB db = new DB();
        [HttpPost("api/jwt/login")]
        public object PostJwtLogin([FromBody] JwtLoginData? loginData)
        {
            if (loginData == null || loginData.username == null || loginData.password == null) return BadRequest(new { Error = "Body is null." });

            var user = db.Products.ToList().FirstOrDefault(a => a.ProductName == loginData.username && a.ProductName == loginData.password);
            if (user == null) return NotFound(new { Error = "User not found." });

            var key = new string('a', 100);

            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    claims: new Claim[] { new Claim(ClaimTypes.NameIdentifier, user.ProductId.ToString()) },
                    expires: expires,
                    signingCredentials: new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
                    )
                );

            return new
            {
                UserId = user.ProductId,
                Expires = expires,
                Token = token,
            };
        }

        [HttpGet("api/products")]
        public object GetProducts()
        {
            return db.Products.ToList().Select(a => new
            {
                a.ProductId,
                a.ProductName,
                a.Stock,
                a.Price,
                CreatedAt = a.CreatedAt.ToString()
            });
        }

        [HttpGet("api/products/{productId}")]
        public object GetProduct(int productId)
        {
            var product = db.Products.ToList().FirstOrDefault(a => a.ProductId == productId);
            if (product == null) return NotFound(new { Error = "Product not found." });
            return new
            {
                product.ProductId,
                product.ProductName,
                product.Stock,
                product.Price,
                CreatedAt = product.CreatedAt.ToString()

            };
        }

        [HttpPost("api/products")]
        public object PostProduct([FromBody] PostProduct? product)
        {
            if (product == null || product.ProductName == null || product.Stock == null || product.Price == null) return BadRequest(new { Error = "Body is null." });
            var newProduct = new Product { ProductName = product.ProductName, Price = product.Price.Value, Stock = product.Stock.Value, CreatedAt = DateTime.Now, ProductId = 0 };
            db.Products.Add(newProduct);
            db.SaveChanges();
            return Created($"api/products/{newProduct.ProductId}", new
            {
                newProduct.ProductId,
                newProduct.ProductName,
                newProduct.Stock,
                newProduct.Price,
                CreatedAt = newProduct.CreatedAt.ToString()
            });
        }
        [HttpPut("api/products/{id}")]
        public object PostProduct(int id, [FromBody] PostProduct? product)
        {
            if (product == null || product.ProductName == null || product.Stock == null || product.Price == null) return BadRequest(new { Error = "Body is null." });
            var updateProduct = db.Products.ToList().FirstOrDefault(a => a.ProductId == id);
            if (updateProduct == null) return NotFound(new { Error = "Product not found." });
            updateProduct.ProductName = product.ProductName;
            updateProduct.Stock = product.Stock.Value;
            updateProduct.Price = product.Price.Value;
            db.SaveChanges();
            return new
            {
                updateProduct.ProductId,
                updateProduct.ProductName,
                updateProduct.Stock,
                updateProduct.Price,
                CreatedAt = updateProduct.CreatedAt.ToString()
            };
        }

        [HttpDelete("api/products/{id}")]
        public object DeleteProduct(int id)
        {
            var updateProduct = db.Products.ToList().FirstOrDefault(a => a.ProductId == id);
            if (updateProduct == null) return NotFound(new { Error = "Product not found." });
            db.Products.Remove(updateProduct);
            db.SaveChanges();
            return NoContent();
        }

        [HttpGet("api/history")]
        public object GetHistory(int limit)
        {
            return db.StockHistories.Include(a => a.Product).ToList().OrderByDescending(a => a.CreatedAt).Take(limit).Select(a => new
            {
                a.HistoryId,
                a.ProductId,
                a.Product.ProductName,
                a.ActionType,
                a.Amount,
                a.Memo,
                CreatedAt = a.CreatedAt.ToString()
            });
        }

        [HttpPost("api/history")]
        public object PostHistory([FromBody] PostHistory? history)
        {
            if (history == null || history.ProductId == null || history.ActionType == null || history.Amount == null || history.Memo == null) return BadRequest(new { Error = "Body is null." });
            if (!db.Products.ToList().Any(a => a.ProductId == history.ProductId.Value)) return NotFound(new { Error = "Product not found." });
            var newHistory = new StockHistory { ProductId = history.ProductId.Value, ActionType = history.ActionType, Amount = history.Amount.Value, Memo = history.Memo, CreatedAt = DateTime.Now };
            db.StockHistories.Add(newHistory);
            db.SaveChanges();
            return Created("", new
            {
                newHistory.HistoryId,
                newHistory.ProductId,
                newHistory.Product.ProductName,
                newHistory.ActionType,
                newHistory.Amount,
                newHistory.Memo,
                CreatedAt = newHistory.CreatedAt.ToString()
            });
        }

        [HttpGet("api/dashboard")]
        public object GetDashboard()
        {
            return new
            {
                ProductCount = db.Products.ToList().Count(),
                TotalStock = db.Products.ToList().Sum(a => a.Stock),
                RecentOperations = db.StockHistories.Include(a => a.Product).ToList().OrderByDescending(a => a.CreatedAt).Take(5).Select(a => new
                {
                    CreatedAt = a.CreatedAt.ToString(),
                    a.Product.ProductName,
                    a.ActionType
                })
            };
        }
    }
}
