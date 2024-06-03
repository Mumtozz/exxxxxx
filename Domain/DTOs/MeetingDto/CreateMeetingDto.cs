namespace Domain.DTOs.MeetingDto;

public class CreateMeetingDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }
}
