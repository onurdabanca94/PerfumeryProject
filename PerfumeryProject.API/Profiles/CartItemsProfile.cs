using AutoMapper;
using PerfumeryProject.API.DTOs.CartItems;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.API.Profiles
{
    public class CartItemsProfile : Profile
    {
        public CartItemsProfile()
        {
            this.CreateMap<CartItem, CreateCartItemsDto>().ReverseMap();
            this.CreateMap<CartItem, SaveOrUpdateCartItemDto>().ReverseMap();
        }
    }
}
