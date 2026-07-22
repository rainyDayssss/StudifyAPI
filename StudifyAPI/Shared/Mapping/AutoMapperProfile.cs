using AutoMapper;
using StudifyAPI.Features.Auth.DTOs;
using StudifyAPI.Features.FriendRequests.DTO;
using StudifyAPI.Features.FriendRequests.Model;
using StudifyAPI.Features.Friends.DTO;
using StudifyAPI.Features.Friends.Model;
using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Features.Users.Models;
using StudifyAPI.Features.UserStreaks.DTO;
using StudifyAPI.Features.UserStreaks.Model;

namespace StudifyAPI.Shared.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Users
            CreateMap<User, UserReadDTO>()
                .ForMember(dest => dest.CurrentStreakDays, opt => opt.MapFrom(src => src.Streak != null ? src.Streak.CurrentStreakDays : 0));
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserPatchDTO, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null values for patch

            // Tasks
            CreateMap<UserTask, UserTaskReadDTO>();
            CreateMap<UserTaskCreateDTO, UserTask>();
            CreateMap<UserTaskPatchDTO, UserTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Streaks
            CreateMap<UserStreak, UserStreakReadDTO>();

            // Friends
            CreateMap<Friend, FriendReadDTO>()
                // Since Friend is a many-to-many join, we often return it from a specific user's perspective.
                // The service will manually handle these specifics if they are complex, or we can map basics.
                .ReverseMap();

            CreateMap<FriendCreateDTO, Friend>();

            // Friend Requests
            CreateMap<FriendRequest, FriendRequestReadDTO>()
                .ForMember(dest => dest.SenderFirstName, opt => opt.MapFrom(src => src.Sender.Firstname))
                .ForMember(dest => dest.SenderLastName, opt => opt.MapFrom(src => src.Sender.Lastname))
                .ForMember(dest => dest.ReceiverFirstName, opt => opt.MapFrom(src => src.Receiver.Firstname));
        }
    }
}