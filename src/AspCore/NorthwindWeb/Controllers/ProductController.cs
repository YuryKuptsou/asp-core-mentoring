using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using NorthwindWeb.Infrastructure.Entities;
using NorthwindWeb.Infrastructure.Interfaces;
using NorthwindWeb.Infrastructure.Options;
using NorthwindWeb.Models;

namespace NorthwindWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly ICategoryService _categoryService;
        private readonly ProductOptions _options;

        public ProductController(IProductService productService, ISupplierService supplierService, ICategoryService categoryService,
            IOptionsSnapshot<ProductOptions> options)
        {
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _options = options.Value;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAll(_options.ProductCount).Select(s => MapProductVM(s));

            return View(products);
        }

        public IActionResult Create()
        {
            var model = new ProductViewModel
            {
                Categories = GetCategories(),
                Suppliers = GetSuppliers()
            };
            ViewBag.Caption = "Create product";

            return View("Update", model);
        }

        public IActionResult Update(int id)
        {
            var product = _productService.Get(id);
            if (product == null)
            {
                return NoContent();
            }

            var model = MapProductVM(product);
            model.Suppliers = GetSuppliers();
            model.Categories = GetCategories();
            ViewBag.Caption = "Update product";

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = GetCategories();
                model.Suppliers = GetSuppliers();

                return View(model);
            }

            if (model.Id == 0)
            {
                _productService.Create(MapProduct(model));
            }
            else
            {
                _productService.Update(MapProduct(model));
            }

            return RedirectToAction("Index");
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            var categories = _categoryService.GetAll().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.CategoryName }).ToList();
            categories.Insert(0, new SelectListItem { Value = "0", Text = "Select category" });

            return categories;
        }

        private IEnumerable<SelectListItem> GetSuppliers()
        {
            var suppliers = _supplierService.GetAll().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.CompanyName }).ToList();
            suppliers.Insert(0, new SelectListItem { Value = "0", Text = "Select supplier" });

            return suppliers;
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

        private Product MapProduct(ProductViewModel productViewModel)
        {
            return new Product
            {
                Id = productViewModel.Id,
                ProductName = productViewModel.ProductName,
                QuantityPerUnit = productViewModel.QuantityPerUnit,
                UnitPrice = productViewModel.UnitPrice,
                UnitsInStock = productViewModel.UnitsInStock,
                UnitsOnOrder = productViewModel.UnitsOnOrder,
                ReorderLevel = productViewModel.ReorderLevel,
                Discontinued = productViewModel.Discontinued,
                CategoryId = productViewModel.CategoryId,
                SupplierId = productViewModel.SupplierId
            };
        }
    }
}