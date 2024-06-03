namespace Domain.DTOs.NotificationDto;

public class CreateNotificationDto
{
    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public DateTime DateOfDispatch { get; set; }
}
