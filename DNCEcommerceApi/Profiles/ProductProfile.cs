using AutoMapper;
using DNCEcommerceApi.Data.Dtos;
using DNCEcommerceApi.Data.Models;

namespace DNCEcommerceApi.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();

        CreateMap<CreateProductWarehouseDto, Warehouse>();

        CreateMap<UpdateProductDto, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UpdateProductWarehouseDto, Warehouse>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Product, ReadProductDto>()
            .ForMember(dest => dest.Inventory, opt => new CreateProductInventoryDto())
            .ForPath(dest => dest.Inventory.Warehouses, opt => opt.MapFrom(src => src.Warehouses));

        CreateMap<Warehouse, ReadProductWarehouseDto>();
    }
}

