namespace Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime? RegisterDate { get; set; }
    public DateTimeOffset CodeTime { get; set; }
    public string? Code { get; set; }


    public List<UserRole>? UserRoles { get; set; }
    public List<Meeting>? Meetings { get; set; }
    public List<Notification>? Notifications { get; set; }

}
