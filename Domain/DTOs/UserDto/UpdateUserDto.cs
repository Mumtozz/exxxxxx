using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.UserDto;

public class UpdateUserDto
{  
     public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime RegisterDate { get; set; }
}
