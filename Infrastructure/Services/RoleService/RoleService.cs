using System.Net;
using AutoMapper;
using Domain.DTOs.RoleDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.RoleService;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly ILogger<RoleService> _logger;

    public RoleService(IMapper mapper, DataContext context, ILogger<RoleService> logger)
    {
        _mapper = mapper;
        _context = context;
        _logger = logger;
    }

    public async Task<Response<string>> AddRoleAsync(CreateRoleDto addRoleDto)
    {
        try
        {
            var mapped = _mapper.Map<Role>(addRoleDto);
            await _context.Roles.AddAsync(mapped);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Success method Add in : {DateTime}", DateTime.UtcNow);
            return new Response<string>("Role added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in method Add in time : {DateTime}", DateTime.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetRoleDto>>> GetRoleAsync(RoleFilter filter)
    {
        try
        {
            var Roles = _context.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                Roles = Roles.Where(x => x.Name == filter.Name);

            var Role = await Roles.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await Roles.CountAsync();
            var response = _mapper.Map<List<GetRoleDto>>(Roles);

            _logger.LogInformation("Success method Get in : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetRoleDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method Get in time : {DateTime}", DateTime.UtcNow);
            return new PagedResponse<List<GetRoleDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }




    public async Task<Response<string>> UpdateRoleAsync(UpdateRoleDto updateRoleDto)
    {
        try
        {
            var existing = await _context.Roles.AnyAsync(e => e.Id == updateRoleDto.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Role not found!");
            var mapped = _mapper.Map<Role>(updateRoleDto);
            _context.Roles.Update(mapped);
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

    public async Task<Response<bool>> DeleteRoleAsync(int id)
    {
        try
        {
            var existing = await _context.Roles.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0) return new Response<bool>(HttpStatusCode.BadRequest, "Role not found!");
            _logger.LogInformation("Success method Delete in : {DateTime}", DateTime.UtcNow);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in method Delete in time : {DateTime}", DateTime.UtcNow);
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<GetRoleDto>> GetRoleByIdAsync(int id)
    {
        try
        {
            var existing = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return new Response<GetRoleDto>(HttpStatusCode.BadRequest, "Role not found");
            var Role = _mapper.Map<GetRoleDto>(existing);

            _logger.LogInformation("Success method GetById in : {DateTime}", DateTime.UtcNow);
            return new Response<GetRoleDto>(Role);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in method GetById in time : {DateTime}", DateTime.UtcNow);
            return new Response<GetRoleDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}