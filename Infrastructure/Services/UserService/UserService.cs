using System.Net;
using AutoMapper;
using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.UserService;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(IMapper mapper, DataContext context, ILogger<UserService> logger)
    {
        _mapper = mapper;
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResponse<List<GetUserDto>>> GetUserAsync(UserFilter filter)
    {
        try
        {
            var Users = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Name))
                Users = Users.Where(x => x.Name == filter.Name);

            var response = _mapper.Map<List<GetUserDto>>(Users);
            var User = await Users.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await Users.CountAsync();
            _logger.LogInformation("Success method Get in : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetUserDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method Get in time : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetUserDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }


    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        try
        {
            var existing = await _context.Users.AnyAsync(e => e.Id == updateUserDto.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "User not found!");
            var mapped = _mapper.Map<User>(updateUserDto);
            _context.Users.Update(mapped);
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

    public async Task<Response<GetUserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var existing = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return new Response<GetUserDto>(HttpStatusCode.BadRequest, "User not found");
            var User = _mapper.Map<GetUserDto>(existing);

            _logger.LogInformation("Success method GetById in : {DateTime}", DateTime.UtcNow);
            return new Response<GetUserDto>(User);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method GetById in time : {DateTime}", DateTime.UtcNow);
            return new Response<GetUserDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}