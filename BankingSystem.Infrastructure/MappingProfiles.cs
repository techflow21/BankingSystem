using AutoMapper;
using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Core.DTOs.Responses;
using BankingSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BankingSystem.Infrastructure
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            /*CreateMap<User, UserRequest>()
                .ForMember(c => c.FullAddress,
                    opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<UserRegistrationDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));*/

            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<User, UserRegistrationDto>().ReverseMap();
            CreateMap<UserResponse, User>().ReverseMap();

            CreateMap<DepositRequest, User>().ReverseMap();
            CreateMap<LoginRequest, User>().ReverseMap();
            CreateMap<IdentityRole, RoleRequest>().ReverseMap();
        }
    }
}
