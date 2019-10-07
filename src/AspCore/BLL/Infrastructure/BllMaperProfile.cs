using AutoMapper;
using BLL.DTO;
using DAL.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Infrastructure
{
    public class BllMaperProfile : Profile
    {
        public BllMaperProfile()
        {
            CreateMap<Category, CategoryDTO>();

            CreateMap<Supplier, SupplierDTO>();

            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
