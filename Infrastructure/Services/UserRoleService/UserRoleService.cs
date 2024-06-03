using System.Net;
using AutoMapper;
using Domain.DTOs.UserRoleDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.UserRoleService;

public class UserRoleService : IUserRoleService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly ILogger<UserRoleService> _logger;

    public UserRoleService(IMapper mapper, DataContext context,ILogger<UserRoleService> logger)
    {
        _mapper = mapper;
        _context = context;
        _logger = logger;
    }
    public async Task<Response<string>> AddUserRole(CreateUserRoleDto addUserRoleDto)
    {
        try
        {
            var mapped = _mapper.Map<UserRole>(addUserRoleDto);
            await _context.UserRoles.AddAsync(mapped);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Success method Add in : {DateTime}", DateTime.UtcNow);
            return new Response<string>("UserRole added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in method Add in time : {DateTime}", DateTime.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetUserRoleDto>>> GetUserRole(UserRoleFilter filter)
    {
        try
        {
            var UserRoles = _context.UserRoles.AsQueryable();
            var UserRole = await UserRoles.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await UserRoles.CountAsync();
            var response = _mapper.Map<List<GetUserRoleDto>>(UserRoles);

            _logger.LogInformation("Success method Get in : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetUserRoleDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method Get in time : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetUserRoleDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteUserRole(int id)
    {
        try
        {
            var existing = await _context.UserRoles.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0) return new Response<bool>(HttpStatusCode.BadRequest, "UserRole not found!");
            _logger.LogInformation("Success method Delete in : {DateTime}", DateTime.UtcNow);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in method Delete in time : {DateTime}", DateTime.UtcNow);
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<GetUserRoleDto>> GetUserRoleById(int id)
    {
        try
        {
            var existing = await _context.UserRoles.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return new Response<GetUserRoleDto>(HttpStatusCode.BadRequest, "UserRole not found");
            var UserRole = _mapper.Map<GetUserRoleDto>(existing);
            
            _logger.LogInformation("Success method GetById in : {DateTime}", DateTime.UtcNow);
            return new Response<GetUserRoleDto>(UserRole);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method GetById in time : {DateTime}", DateTime.UtcNow);
            return new Response<GetUserRoleDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}