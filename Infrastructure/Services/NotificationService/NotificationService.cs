using System.Net;
using AutoMapper;
using Domain.DTOs.NotificationDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.NotificationService;

public class NotificationService : INotificationService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IMapper mapper, DataContext context, ILogger<NotificationService> logger)
    {
        _mapper = mapper;
        _context = context;
        _logger = logger;
    }


    public async Task<Response<string>> AddNotificationAsync(CreateNotificationDto createNotification)
    {
        try
        {
            var mapped = _mapper.Map<Notification>(createNotification);
            await _context.Notifications.AddAsync(mapped);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Success method Add in : {DateTime}", DateTime.UtcNow);
            return new Response<string>("Notification added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in method Add in time : {DateTime}", DateTime.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetNotificationDto>>> GetNotificationAsync(NotificationFilter filter)
    {
        try
        {
            var Notifications = _context.Notifications.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Message))
                Notifications = Notifications.Where(x => x.Message == filter.Message);

            var response = _mapper.Map<List<GetNotificationDto>>(Notifications);
            var Notification = await Notifications.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await Notifications.CountAsync();
            _logger.LogInformation("Success method Get in : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetNotificationDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method Get in time : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetNotificationDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }


    public async Task<Response<GetNotificationDto>> GetNotificationByIdAsync(int id)
    {
        try
        {
            var existing = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return new Response<GetNotificationDto>(HttpStatusCode.BadRequest, "Notification not found");
            var Notification = _mapper.Map<GetNotificationDto>(existing);

            _logger.LogInformation("Success method GetById in : {DateTime}", DateTime.UtcNow);
            return new Response<GetNotificationDto>(Notification);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method GetById in time : {DateTime}", DateTime.UtcNow);
            return new Response<GetNotificationDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}