namespace EchoShelf_Api.Entities
{
    public class RequestSubmitMemory
    {
        public string Title { get; set; }
        public int UserId { get; set; }
        public bool IsFavorite { get; set; }
        public int CategoryId { get; set; }
        public string MemoryDate { get; set; }
        public string? ImagePath { get; set; }
        public string Episopde{ get; set; }
        public List<int> Tags { get; set; }
    }
}
