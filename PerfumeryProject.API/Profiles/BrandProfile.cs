using AutoMapper;
using PerfumeryProject.API.DTOs.Brand;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.API.Profiles
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            this.CreateMap<Brand, CreateBrandDto>().ReverseMap();
        }
    }
}
