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
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IRepository<Category> _categoryRepository;

        public ProductService(IMapper mapper, IRepository<Product> productRepository,
            IRepository<Supplier> supplierRepository, IRepository<Category> categoryRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
        }

        public ProductDTO Get(int id)
        {
            if (_productRepository.Get(id) == null)
            {
                return null;
            }

            var _product = from product in new[] { _productRepository.Get(id) }
                           join supplier in _supplierRepository.GetAll() on product.SupplierID equals supplier.SupplierID
                           join category in _categoryRepository.GetAll() on product.CategoryID equals category.CategoryID
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
                               SupplierID = supplier.SupplierID,
                               CompanyName = supplier.CompanyName,
                               CategoryID = category.CategoryID,
                               CategoryName = category.CategoryName
                           };

            return _product.FirstOrDefault();
        }

        public IEnumerable<ProductDTO> GetAll(int count = 0)
        {
            if (count < 0)
            {
                throw new ArgumentException();
            }

            var products = count == 0 ? _productRepository.GetAll() : _productRepository.Take(count);

            var productsWithDetails = from product in products
                           join supplier in _supplierRepository.GetAll() on product.SupplierID equals supplier.SupplierID
                           join category in _categoryRepository.GetAll() on product.CategoryID equals category.CategoryID
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

            return productsWithDetails;
        }

        public int CreateOrUpdate(ProductDTO product)
        {
            int id = product.ProductID;
            if (product.ProductID == 0)
            {
                id = _productRepository.Create(_mapper.Map<Product>(product)).ProductID;
            }
            else
            {
                _productRepository.Update(_mapper.Map<Product>(product));
            }

            return id;
        }

        public void Remove(ProductDTO product)
        {
            _productRepository.Delete(_mapper.Map<Product>(product));
        }
    }
}
