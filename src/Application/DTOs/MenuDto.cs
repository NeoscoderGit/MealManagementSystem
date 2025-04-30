
namespace Application.DTOs
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public int? ParentId { get; set; }
    }
}
