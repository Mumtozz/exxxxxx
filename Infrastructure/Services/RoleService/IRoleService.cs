using Domain.DTOs.RoleDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.RoleService;

public interface IRoleService
{
    Task<PagedResponse<List<GetRoleDto>>> GetRoleAsync(RoleFilter filter);
    Task<Response<string>> AddRoleAsync(CreateRoleDto createRoleDto);
    Task<Response<string>> UpdateRoleAsync(UpdateRoleDto updateRoleDto);
    Task<Response<bool>> DeleteRoleAsync(int id);
    Task<Response<GetRoleDto>> GetRoleByIdAsync(int id);
}
