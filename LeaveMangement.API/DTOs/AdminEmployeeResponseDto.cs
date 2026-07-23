namespace LeaveMangement.API.DTOs
{
    public class AdminEmployeeResponseDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public string? ManagerName { get; set; }

        public bool IsManager { get; set; }
    }
}