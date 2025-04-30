
using Domain.Entitys.Common;

namespace Domain.Entitys.AuthModel
{
    public class RoleMenu : BaseEntity
    {
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public int? MenuId { get; set; }
        public Menu? Menu { get; set; }
    }
}
