using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Domains;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IMapper mapper, IProductRepository productRepository
            ,ISupplierRepository supplierRepository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
        }

        public ProductDTO Get(int id)
        {
            var _product = from product in new[] { _productRepository.Get(id) }
                           join supplier in _supplierRepository.GetAll() on product.SupplierId equals supplier.SupplierID
                           join category in _categoryRepository.GetAll() on product.CategoryId equals category.CategoryID
                           select new ProductDTO
                           {
                               ProductID = product.ProductID,
                               ProductName = product.ProductName,
                               QuantityPerUnit = product.QuantityPerUnit,
                               UnitPrice = product.UnitPrice,
                               UnitsInStock = product.UnitsInStock,
                               UnitsOnOrder = product.UnitsOnOrder,
                               ReorderLevel = product.ReorderLevel,
                               Discontinued = product.Discontinued,
                               CompanyName = supplier.CompanyName,
                               CategoryName = category.CategoryName
                           };

            return _product.FirstOrDefault();
        }

        public IEnumerable<ProductDTO> GetAll(int count)
        {
            var products = from product in _productRepository.GetAll(count)
                           join supplier in _supplierRepository.GetAll() on product.SupplierId equals supplier.SupplierID
                           join category in _categoryRepository.GetAll() on product.CategoryId equals category.CategoryID
                           select new ProductDTO
                           {
                               ProductID = product.ProductID,
                               ProductName = product.ProductName,
                               QuantityPerUnit = product.QuantityPerUnit,
                               UnitPrice = product.UnitPrice,
                               UnitsInStock = product.UnitsInStock,
                               UnitsOnOrder = product.UnitsOnOrder,
                               ReorderLevel = product.ReorderLevel,
                               Discontinued = product.Discontinued,
                               CompanyName = supplier.CompanyName,
                               CategoryName = category.CategoryName
                           };

            return products;
        }

        public void Update(ProductDTO product)
        {
            _productRepository.Update(_mapper.Map<Product>(product));
        }

        public int Create(ProductDTO product)
        {
            return _productRepository.Create(_mapper.Map<Product>(product));
        }
    }
}
