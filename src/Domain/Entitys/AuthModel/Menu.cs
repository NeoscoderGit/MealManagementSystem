
using Domain.Entitys.Common;

namespace Domain.Entitys.AuthModel
{
    public class Menu : BaseEntity
    {
        public string? Name { get; set; }
        public string? Url { get; set; }

        public int? ParentId { get; set; }
        public Menu? Parent { get; set; }

        public ICollection<Menu> Children { get; set; } = new List<Menu>();

        public bool? IsParrent { get; set; }
        public bool? IsLastChild { get; set; }

        public List<RoleMenu> RoleMenus { get; set; } = new();
    }
}
