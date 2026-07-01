using EchoShelf_Api.Entities;
using EchoShelf_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EchoShelf_Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private DB db = new DB();
        [HttpPost("api/auth/login")]
        public object PostLogin([FromBody] RequestPostLogin loginData)
        {
            var user = db.Users.ToList().FirstOrDefault(a => a.Email == loginData.Email && a.PasswordHash == loginData.Password);
            if (user == null) return Unauthorized(new { Error = "Authentication failed." });

            return new
            {
                Message = "Success",
                Data = user.UserId
            };
        }

        [HttpPost("api/auth/logout")]
        public object PostLogout([FromBody] RequestPostLogout logoutData)
        {
            var user = db.Users.ToList().FirstOrDefault(a => a.UserId == logoutData.UserId);
            if (user == null) return NotFound(new { Error = "User not found." });

            return new
            {
                Message = "Logout",
                Data = user.UserId
            };
        }

        [HttpGet("api/memories")]
        public object GetMemories(int userId)
        {
            var user = db.Users.Include(a => a.Memories).ToList().FirstOrDefault(a => a.UserId == userId);
            if (user == null) return NotFound(new { Error = "User not found." });

            return user.Memories.Select(a => new
            {
                a.MemoryId,
                a.Title,
                MemoryDate = a.MemoryDate.ToString("yyyy-MM-dd")
            });
        }

        [HttpPost("api/memories")]
        public object PostMemories([FromBody] RequestSubmitMemory memory)
        {
            if (memory.Title == "" || memory.MemoryDate == "" || memory.Episopde == "") return BadRequest(new { Error = "Input error." });
            var category = db.Categories.ToList().FirstOrDefault(a => a.CategoryId == memory.CategoryId);
            if (category == null) return NotFound(new { Error = "Category not found." });
            var user = db.Users.ToList().FirstOrDefault(a => a.UserId == memory.UserId);
            if (category == null) return NotFound(new { Error = "Category not found." });

            var newMemory = new Memory { UserId = memory.UserId, CategoryId = memory.CategoryId, Title = memory.Title, Episode = memory.Episopde, MemoryDate = DateOnly.Parse(memory.MemoryDate), IsFavorite = memory.IsFavorite, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false };
            db.Memories.Add(newMemory);
            db.SaveChanges();

            return Created("", db.Memories.ToList().Select(a => new
            {
                a.MemoryId,
                a.Title,
                MemoryDate = a.MemoryDate.ToString("yyyy-MM-dd")
            }));
        }

        [HttpGet("api/categories")]
        public object GetCategories()
        {
            return db.Categories.ToList().Select(a => new
            {
                a.CategoryId,
                a.CategoryName
            });
        }
        [HttpGet("api/tags")]
        public object GetTags()
        {
            return db.Tags.ToList().Select(a => new
            {
                a.TagId,
                a.TagName,
            });
        }

        [HttpDelete("api/memories/{memoryId}")]
        public object DeleteMemory(int memoryId)
        {
            var memory = db.Memories.ToList().FirstOrDefault(a => a.MemoryId == memoryId);
            if (memory == null) return NotFound(new { Error = "Memory not found." });

            db.Memories.Remove(memory);
            db.SaveChanges();

            return new
            {
                Message = "Deleted"
            };
        }

    }
}
