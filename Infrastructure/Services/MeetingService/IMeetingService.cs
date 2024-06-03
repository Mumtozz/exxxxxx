using Domain.DTOs.MeetingDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.MeetingService;

public interface IMeetingService
{
    Task<PagedResponse<List<GetMeetingDto>>> GetMeetingAsync(MeetingFilter filter);
    Task<Response<string>> AddMeetingAsync(CreateMeetingDto createMeetingDto);
    Task<Response<string>> UpdateMeetingAsync(UpdateMeetingDto updateMeetingDto);
    Task<Response<bool>> DeleteMeetingAsync(int id);
    Task<Response<GetMeetingDto>> GetMeetingByIdAsync(int id);
    Task<PagedResponse<List<GetMeetingDto>>> GetUpcomingMeetings(PaginationFilter filter, int userId);
}
