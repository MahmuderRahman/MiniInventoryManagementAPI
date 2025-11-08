using AutoMapper;
using MiniInventoryManagementAPI.DTOs;
using MiniInventoryManagementAPI.Models;

namespace MiniInventoryManagementAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ReverseMap();


        }
    }
}
