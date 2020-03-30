using System;
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
            Mapper.Initialize(cfg);
            Mapper.AssertConfigurationIsValid();
        }
    }
}
