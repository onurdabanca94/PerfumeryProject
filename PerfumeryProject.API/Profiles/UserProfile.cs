using AutoMapper;
using PerfumeryProject.API.DTOs.User;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, CreateUserDto>().ReverseMap();
        }
    }
}
