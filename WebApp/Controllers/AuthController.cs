using System.Net;
using Domain.DTOs.AuthDTOs;
using Domain.Responses;
using Infrastructure.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var response = await authService.Login(loginDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var response = await authService.Register(registerDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("change-password")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value);
        var result = await authService.ChangePassword(changePasswordDto, userId!);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        var result = await authService.ForgotPassword(forgotPasswordDto);
        return StatusCode(result.StatusCode, result);
    }


    [HttpDelete("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var result = await authService.ResetPassword(resetPasswordDto);
        return StatusCode(result.StatusCode, result);
    }


    [HttpDelete("delete-account")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value);
        var result = await authService.DeleteAccount(userId!);
        return StatusCode(result.StatusCode, result);
    }
}