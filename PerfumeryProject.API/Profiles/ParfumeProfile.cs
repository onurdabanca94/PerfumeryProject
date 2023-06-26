using AutoMapper;
using PerfumeryProject.API.DTOs.Parfume;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.API.Profiles
{
    public class ParfumeProfile : Profile
    {
        public ParfumeProfile()
        {
            this.CreateMap<Parfum, CreateParfumeDto>().ReverseMap();
        }
    }
}
