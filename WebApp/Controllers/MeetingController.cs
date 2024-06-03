using Domain.Constants;
using Domain.DTOs.MeetingDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.MeetingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeetingController(IMeetingService MeetingService) : ControllerBase
{
    [HttpGet("Get-Meeting")]
    [PermissionAuthorize(Permissions.Meetings.View)]
    public async Task<Response<List<GetMeetingDto>>> GetMeetingsAsync([FromQuery] MeetingFilter filter)
    {
        return await MeetingService.GetMeetingAsync(filter);
    }

    [HttpGet("Get-Meeting-by-id")]
    [PermissionAuthorize(Permissions.Meetings.View)]
    public async Task<Response<GetMeetingDto>> GetMeetingByIdAsync(int MeetingId)
    {
        return await MeetingService.GetMeetingByIdAsync(MeetingId);
    }

    [HttpPost("create - Meeting")]
    [PermissionAuthorize(Permissions.Meetings.Create)]
    public async Task<Response<string>> CreateMeetingAsync(CreateMeetingDto Meeting)
    {
        return await MeetingService.AddMeetingAsync(Meeting);
    }


    [HttpPut("update - Meeting")]
    [PermissionAuthorize(Permissions.Meetings.Edit)]
    public async Task<Response<string>> UpdateMeetingAsync(UpdateMeetingDto Meeting)
    {
        return await MeetingService.UpdateMeetingAsync(Meeting);
    }

    [HttpDelete("Delete Meeting")]
    [PermissionAuthorize(Permissions.Meetings.Delete)]
    public async Task<Response<bool>> DeleteMeetingAsync(int MeetingId)
    {
        return await MeetingService.DeleteMeetingAsync(MeetingId);
    }

    [HttpGet("GetUpcomingMeetingsAsync")]
    [PermissionAuthorize(Permissions.Meetings.View)]
    public async Task<IActionResult> GetUpcomingMeetings(PaginationFilter filter)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "sid")?.Value);
        var result = await MeetingService.GetUpcomingMeetings(filter,userId);
        return StatusCode(result.StatusCode,result);
    }
}