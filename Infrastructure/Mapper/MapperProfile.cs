using AutoMapper;
using Domain.DTOs.RoleDto;
using Domain.DTOs.UserDto;
using Domain.DTOs.UserRoleDto;

using Domain.Entities;
using Domain.DTOs.NotificationDto;
using Domain.DTOs.MeetingDto;

namespace Infrastructure.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {


        CreateMap<Role, GetRoleDto>().ReverseMap();
        CreateMap<Role, CreateRoleDto>().ReverseMap();
        CreateMap<Role, UpdateRoleDto>().ReverseMap();

        CreateMap<Meeting, GetMeetingDto>().ReverseMap();
        CreateMap<Meeting, CreateMeetingDto>().ReverseMap();
        CreateMap<Meeting, UpdateMeetingDto>().ReverseMap();

        CreateMap<Notification, GetNotificationDto>().ReverseMap();
        CreateMap<Notification, CreateNotificationDto>().ReverseMap();

        CreateMap<User, GetUserDto>().ReverseMap();
        CreateMap<User, UpdateUserDto>().ReverseMap();

        CreateMap<UserRole, GetUserRoleDto>().ReverseMap();
        CreateMap<UserRole, CreateUserRoleDto>().ReverseMap();

    }
}