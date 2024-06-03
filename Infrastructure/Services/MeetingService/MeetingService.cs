using System.Net;
using AutoMapper;
using Domain.DTOs.MeetingDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;

namespace Infrastructure.Services.MeetingService;

public class MeetingService : IMeetingService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly ILogger<MeetingService> _logger;


    public MeetingService(IMapper mapper, DataContext context, ILogger<MeetingService> logger)
    {
        _mapper = mapper;
        _context = context;
        _logger = logger;

    }
    public async Task<Response<string>> AddMeetingAsync(CreateMeetingDto addMeetingDto)
    {
        try
        {
            var mapped = _mapper.Map<Meeting>(addMeetingDto);
            await _context.Meetings.AddAsync(mapped);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Success method Add in : {DateTime}", DateTime.UtcNow);
            return new Response<string>("Meeting added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in method Add in time : {DateTime}", DateTime.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetMeetingDto>>> GetMeetingAsync(MeetingFilter filter)
    {
        try
        {
            var Meetings = _context.Meetings.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                Meetings = Meetings.Where(x => x.Name == filter.Name);

            if (!string.IsNullOrEmpty(filter.Description))
                Meetings = Meetings.Where(x => x.Description == filter.Description);

            var Meeting = await Meetings.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await Meetings.CountAsync();

            var response = _mapper.Map<List<GetMeetingDto>>(Meetings);
            _logger.LogInformation("Success method Get in : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetMeetingDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method Get in time : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetMeetingDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteMeetingAsync(int id)
    {
        try
        {
            var existing = await _context.Meetings.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0) return new Response<bool>(HttpStatusCode.BadRequest, "Meeting not found!");
            _logger.LogInformation("Success method Delete in : {DateTime}", DateTime.UtcNow);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in method Delete in time : {DateTime}", DateTime.UtcNow);
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }



    public async Task<PagedResponse<List<GetMeetingDto>>> GetUpcomingMeetings(PaginationFilter filter, int userId)
    {
        try
        {
            _logger.LogInformation("Starting method {GetMeetingsAsync} in time:{DateTime} ", "GetMeetingsAsync",
                DateTimeOffset.UtcNow);
            var Meetings = _context.Meetings.AsQueryable();

            var response = await Meetings.Select(x => new GetMeetingDto()
            {
                Name = x.Name,
                Description = x.Description,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                UserId = x.UserId,

                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).Where(x => x.UserId == userId && x.StartDate > DateTime.UtcNow).ToListAsync();

            var totalRecord = await Meetings.CountAsync();

            _logger.LogInformation("Finished method {GetMeetingsAsync} in time:{DateTime} ", "GetMeetingsAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMeetingDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            _logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMeetingDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<string>> UpdateMeetingAsync(UpdateMeetingDto updateMeetingDto)
    {
        try
        {
            var existing = await _context.Meetings.AnyAsync(e => e.Id == updateMeetingDto.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Meeting not found!");
            var mapped = _mapper.Map<Meeting>(updateMeetingDto);
            _context.Meetings.Update(mapped);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Success method Update in : {DateTime}", DateTime.UtcNow);
            return new Response<string>("Updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in method Update in time : {DateTime}", DateTime.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<GetMeetingDto>> GetMeetingByIdAsync(int id)
    {
        try
        {
            var existing = await _context.Meetings.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null)
            {
                return new Response<GetMeetingDto>(HttpStatusCode.BadRequest, "Meeting not found");
            }
            var Meeting = _mapper.Map<GetMeetingDto>(existing);
            _logger.LogInformation("Success method GetById in : {DateTime}", DateTime.UtcNow);
            return new Response<GetMeetingDto>(Meeting);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method GetById in time : {DateTime}", DateTime.UtcNow);
            return new Response<GetMeetingDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }


    
}