using AutoMapper;
using BLL.DTO;
using Microsoft.AspNetCore.Http;
using NorthwindWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure
{
    public class NorthwindMapperProfile : Profile
    {
        public NorthwindMapperProfile()
        {
            CreateMap<IFormFile, byte[]>().ConvertUsing((s, d) =>
            {
                MemoryStream memoryStream = new MemoryStream();
                s.CopyTo(memoryStream);
                return memoryStream.ToArray();
            });
            CreateMap<CategoryDTO, CategoryViewModel>();
            CreateMap<CategoryViewModel, CategoryDTO>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.Image.ContentType));

            CreateMap<ProductDTO, ProductViewModel>();
            CreateMap<ProductViewModel, ProductDTO>();
        }
    }

}
