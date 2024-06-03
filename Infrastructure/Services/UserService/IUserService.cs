using Domain;
using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.UserService;

public interface IUserService
{
    Task<PagedResponse<List<GetUserDto>>> GetUserAsync(UserFilter filter);
    Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto);
    Task<Response<GetUserDto>> GetUserByIdAsync(int id);
}
