using Domain.Constants;
using Domain.DTOs.NotificationDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController(INotificationService NotificationService) : ControllerBase
{
    [HttpGet("Get-Notification")]
    [PermissionAuthorize(Permissions.Notifications.View)]
    public async Task<Response<List<GetNotificationDto>>> GetNotificationsAsync([FromQuery] NotificationFilter filter)
    {
        return await NotificationService.GetNotificationAsync(filter);
    }

    [HttpGet("Get-Notification-by-id")]
    [PermissionAuthorize(Permissions.Notifications.View)] 
    public async Task<Response<GetNotificationDto>> GetNotificationByIdAsync(int NotificationId)
    {
        return await NotificationService.GetNotificationByIdAsync(NotificationId);
    }

    [HttpPost("create - Notification")]
    [PermissionAuthorize(Permissions.Notifications.Create)]
    public async Task<Response<string>> CreateNotificationAsync(CreateNotificationDto Notification)
    {
        return await NotificationService.AddNotificationAsync(Notification);
    }

}