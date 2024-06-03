using Domain.Constants;
using Domain.DTOs.RoleDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController(IRoleService RoleService) : ControllerBase
{
    [HttpGet("Get-Role")]
    [PermissionAuthorize(Permissions.Roles.View)]
    public async Task<Response<List<GetRoleDto>>> GetRolesAsync([FromQuery] RoleFilter filter)
    {
        return await RoleService.GetRoleAsync(filter);
    }

    [HttpGet("Get-Role-by-id")]
    [PermissionAuthorize(Permissions.Roles.View)] 
    public async Task<Response<GetRoleDto>> GetRoleByIdAsync(int RoleId)
    {
        return await RoleService.GetRoleByIdAsync(RoleId);
    }

    [HttpPost("create - Role")]
    [PermissionAuthorize(Permissions.Roles.Create)]
    public async Task<Response<string>> CreateRoleAsync(CreateRoleDto Role)
    {
        return await RoleService.AddRoleAsync(Role);
    }


    [HttpPut("update - Role")]
    [PermissionAuthorize(Permissions.Roles.Edit)]
    public async Task<Response<string>> UpdateRoleAsync(UpdateRoleDto Role)
    {
        return await RoleService.UpdateRoleAsync(Role);
    }

    [HttpDelete("Delete Role")]
    [PermissionAuthorize(Permissions.Roles.Delete)]
    public async Task<Response<bool>> DeleteRoleAsync(int RoleId)
    {
        return await RoleService.DeleteRoleAsync(RoleId);
    }
}