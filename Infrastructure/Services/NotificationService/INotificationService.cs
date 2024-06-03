using Domain.DTOs.NotificationDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.NotificationService;

public interface INotificationService
{
    Task<PagedResponse<List<GetNotificationDto>>> GetNotificationAsync(NotificationFilter filter);
    Task<Response<string>> AddNotificationAsync(CreateNotificationDto createNotificationDto);
    Task<Response<GetNotificationDto>> GetNotificationByIdAsync(int id);
}
