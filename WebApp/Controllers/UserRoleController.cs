using Domain.Constants;
using Domain.DTOs.UserRoleDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.UserRoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserRoleController(IUserRoleService UserRoleService) : ControllerBase
{
    [HttpGet("Get-UserRole")]
    [PermissionAuthorize(Permissions.UserRoles.View)]
    public async Task<Response<List<GetUserRoleDto>>> GetUserRolesAsync([FromQuery] UserRoleFilter filter)
    {
        return await UserRoleService.GetUserRole(filter);
    }

    [HttpGet("Get-UserRole-by-id")]
    [PermissionAuthorize(Permissions.UserRoles.View)] 
    public async Task<Response<GetUserRoleDto>> GetUserRoleByIdAsync(int UserRoleId)
    {
        return await UserRoleService.GetUserRoleById(UserRoleId);
    }

    [HttpPost("create - UserRole")]
    [PermissionAuthorize(Permissions.UserRoles.Create)]
    public async Task<Response<string>> CreateUserRole(CreateUserRoleDto UserRole)
    {
        return await UserRoleService.AddUserRole(UserRole);
    }

    [HttpDelete("Delete UserRole")]
    [PermissionAuthorize(Permissions.UserRoles.Delete)]
    public async Task<Response<bool>> DeleteUserRole(int UserRoleId)
    {
        return await UserRoleService.DeleteUserRole(UserRoleId);
    }
}