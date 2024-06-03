using Domain.DTOs.UserRoleDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.UserRoleService;

public interface IUserRoleService
{
    Task<PagedResponse<List<GetUserRoleDto>>> GetUserRole(UserRoleFilter filter);
    Task<Response<string>> AddUserRole(CreateUserRoleDto addUserRoleDto);
    Task<Response<bool>> DeleteUserRole(int id);
    Task<Response<GetUserRoleDto>> GetUserRoleById(int id);
}