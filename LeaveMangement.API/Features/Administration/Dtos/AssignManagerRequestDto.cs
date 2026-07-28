namespace LeaveManagement.API.Features.Administration.Dtos
{
    public class AssignManagerRequestDto
    {
        public List<int> ManagerIds { get; set; }
            = new();
    }
}