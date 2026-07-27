namespace LeaveMangement.API.DTOs
{
    public class AdminEmployeeResponseDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public List<string> ManagerNames { get; set; }
            = new();

        public bool IsManager { get; set; }
    }
}