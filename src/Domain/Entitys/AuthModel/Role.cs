
using Domain.Entitys.Common;

namespace Domain.Entitys.AuthModel
{
    public class Role : BaseEntity
    {
        public string? Name { get; set; }
        public List<UserRole>? UserRoles { get; set; } = new();
        public List<RoleMenu>? RoleMenus { get; set; } = new();
    }
}
