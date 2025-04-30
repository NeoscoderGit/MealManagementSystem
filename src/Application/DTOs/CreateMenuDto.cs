
namespace Application.DTOs
{
    public class CreateMenuDto
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        public int? ParentId { get; set; }
    }
}
