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
                .ForMember(x => x.CartItems, opt => opt.Ignore())
                .ForMember(x => x.Variants, opt => opt.Ignore())
                .ForMember(x => x.Department, opt => opt.Ignore())
                .ForMember(x => x.Category, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Category, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(x => x.Department, opt => opt.MapFrom(src => src.Department.DepartmentName));
            //cfg.CreateMap<ICollection<ResponseProductModel>, ICollection<Product>>()
            //    .PreserveReferences()
            //    .ReverseMap()
            //    .PreserveReferences();
            cfg.CreateMap<ResponseDepartmentDto, Department>()
               .ForMember(x => x.Products, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(x => x.Categories, opt => opt.MapFrom(src => src.Categories));
            cfg.CreateMap<Cart, CartModel>()
                .ForMember(x => x.Items, opt => opt.Ignore())
                .ForMember(x => x.increase, opt => opt.Ignore())
                .ForMember(x => x.decrease, opt => opt.Ignore());
            cfg.CreateMap<CartItem, CartItemDto>()
                .ForMember(x => x.Item, opt => opt.MapFrom(src => src.Product));

            Mapper.Initialize(cfg);
            Mapper.AssertConfigurationIsValid();
        }
    }
}
