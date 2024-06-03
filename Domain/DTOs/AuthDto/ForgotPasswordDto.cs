using System.ComponentModel.DataAnnotations;
using MimeKit;

namespace Domain.DTOs.AuthDTOs;

public class ForgotPasswordDto
{
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
}