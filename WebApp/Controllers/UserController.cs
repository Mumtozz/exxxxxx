using Domain.Constants;
using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserService UserService) : ControllerBase
{
    [HttpGet("Get-User")]
    [PermissionAuthorize(Permissions.Users.View)]
    public async Task<Response<List<GetUserDto>>> GetUsersAsync([FromQuery] UserFilter filter)
    {
        return await UserService.GetUserAsync(filter);
    }

    [HttpGet("Get-User-by-id")]
    [PermissionAuthorize(Permissions.Users.View)] 
    public async Task<Response<GetUserDto>> GetUserByIdAsync(int UserId)
    {
        return await UserService.GetUserByIdAsync(UserId);
    }


    [HttpPut("update - User")]
    [PermissionAuthorize(Permissions.Users.Edit)]
    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto User)
    {
        return await UserService.UpdateUserAsync(User);
    }

}