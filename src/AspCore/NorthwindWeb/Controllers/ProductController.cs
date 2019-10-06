using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NorthwindWeb.Infrastructure.Entities;
using NorthwindWeb.Infrastructure.Interfaces;
using NorthwindWeb.Models;

namespace NorthwindWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAll().Select(s => MapProductVM(s));

            return View(products);
        }

        private ProductViewModel MapProductVM(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                CompanyName = product.CompanyName,
                CategoryName = product.CategoryName
            };
        }
    }
}