namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }

    public List<UserRole>? UserRoles { get; set; }

}
