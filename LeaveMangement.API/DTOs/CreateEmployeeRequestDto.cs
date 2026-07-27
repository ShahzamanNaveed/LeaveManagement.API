namespace LeaveMangement.API.DTOs
{
    public class CreateEmployeeRequestDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public List<int> ManagerIds { get; set; }
            = new();
    }
}