using AutoMapper;
using BLL.DTO;
using NorthwindWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure
{
    public class NorthwindMapperProfile : Profile
    {
        public NorthwindMapperProfile()
        {
            CreateMap<CategoryDTO, CategoryViewModel>();

            CreateMap<ProductDTO, ProductViewModel>();
            CreateMap<ProductViewModel, ProductDTO>();
        }
    }
}
