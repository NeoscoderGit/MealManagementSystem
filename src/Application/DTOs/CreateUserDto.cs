
namespace Application.DTOs
{
    public class CreateUserDto
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
        public string? Password { get; set; }
        public List<int>? RoleIds { get; set; }
    }
}
