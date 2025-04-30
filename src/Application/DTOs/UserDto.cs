
namespace Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? OfficeEmail { get; set; }
        public string? ContactNo { get; set; }
        public List<string>? Roles { get; set; }
    }
}
