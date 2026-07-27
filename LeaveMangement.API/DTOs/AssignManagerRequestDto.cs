namespace LeaveMangement.API.DTOs
{
    public class AssignManagerRequestDto
    {
        public List<int> ManagerIds { get; set; }
            = new();
    }
}