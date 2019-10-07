using NorthwindWeb.Infrastructure.Entities;
using NorthwindWeb.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _dbContext;

        public ProductService(NorthwindContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<Product> GetAll(int count)
        {
            var products = from product in _dbContext.Products
                           join supplier in _dbContext.Suppliers on product.SupplierId equals supplier.Id
                           join category in _dbContext.Categories on product.CategoryId equals category.Id
                           select new Product
                           {
                               Id = product.Id,
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

            if (count == 0)
            {
                return products.ToList();
            }
            else
            {
                return products.Take(count).ToList();
            }
        }
    }
}
