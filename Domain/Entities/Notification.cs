namespace Domain.Entities;

public class Notification : BaseEntity
{
    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public DateTime DateOfDispatch { get; set; }

    
    public User? User { get; set; }
}
