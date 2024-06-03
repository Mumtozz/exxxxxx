namespace Domain.Entities;

public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    
    public Role? Role { get; set; }
    public User? User { get; set; }
}
