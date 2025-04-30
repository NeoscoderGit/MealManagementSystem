
using Domain.Entitys.Common;

namespace Domain.Entitys.AuthModel
{
    public class User : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? JobStationId { get; set; }
        public int? UnitId { get; set; }
        public string? OfficeEmail { get; set; }
        public string? ContactNo { get; set; }
        public string? CardNumber { get; set; }
        public string? Password { get; set; }
        public List<UserRole>? UserRoles { get; set; } = new();
    }
}
