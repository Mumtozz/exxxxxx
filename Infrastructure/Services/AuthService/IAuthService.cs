using Domain.Responses;
using Domain.DTOs.AuthDTOs;

namespace Infrastructure.Services.AuthService;

public interface IAuthService
{
    Task<Response<string>> Register(RegisterDto model);
    Task<Response<string>> Login(LoginDto login);
    Task<Response<string>> ChangePassword(ChangePasswordDto passwordDto, int userId);
    Task<Response<string>> ResetPassword(ResetPasswordDto resetPasswordDto);
    Task<Response<string>> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
    Task<Response<string>> DeleteAccount(int userId);

}
