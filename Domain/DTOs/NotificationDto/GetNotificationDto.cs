namespace Domain.DTOs.NotificationDto;

public class GetNotificationDto
{
    public int Id { get; set; }
    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public DateTime DateOfDispatch { get; set; }
}
