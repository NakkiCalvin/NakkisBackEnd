using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Requests;
using API.Responses;
using AutoMapper;
using AutoMapper.Configuration;
using BLL.Entities;

namespace API.Mapping
{
    public class AutomapperConfig
    {
        public static void Configure()
        {
            var cfg = new MapperConfigurationExpression();

            cfg.CreateMap<RegisterUserModel, User>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForAllOtherMembers(x => x.Ignore());
            cfg.CreateMap<RequestBookModel, Book>();
            cfg.CreateMap<Book, ResponseBookModel>();
            cfg.CreateMap<ResponseProductModel, Product>()
                .ForMember(x => x.Variants, opt => opt.Ignore())
                .ForMember(x => x.VariantSizes, opt => opt.Ignore())
                .ForMember(x => x.Department, opt => opt.Ignore())
                .ForMember(x => x.Category, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Category, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(x => x.VariantSizes, opt => opt.MapFrom(src => src.VariantSizes))
                .ForMember(x => x.Department, opt => opt.MapFrom(src => src.Department.DepartmentName));
            cfg.CreateMap<ResponseDepartmentDto, Department>()
               .ForMember(x => x.Products, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(x => x.Categories, opt => opt.MapFrom(src => src.Categories));
            cfg.CreateMap<Cart, CartModel>()
                .ForMember(x => x.Items, opt => opt.Ignore())
                .ForMember(x => x.increase, opt => opt.Ignore())
                .ForMember(x => x.decrease, opt => opt.Ignore());

            cfg.CreateMap<CartItem, CartItemDto>()
                .ForMember(x => x.Item, opt => opt.MapFrom(src => new ResponseProductModel { Id = src.AvailabilityId,
                    Color = src.Availability.Variant.Color,
                    Description = src.Availability.Variant.Product.Description,
                    ImagePath = src.Availability.Variant.ImagePath,
                    Price = src.Price,
                    Size = src.Availability.Size.ToString(),
                    Title = src.Availability.Variant.Product.Title,
                    VariantId = src.Availability.VariantId,
                    Quantity = src.Availability.Quantity,
                }));
            cfg.CreateMap<Availability, AvalabilityModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Variant.Product.Title))
                .ForMember(x => x.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(x => x.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(x => x.Color, opt => opt.MapFrom(src => src.Variant.Color));

            Mapper.Initialize(cfg);
            Mapper.AssertConfigurationIsValid();
        }
    }
}
