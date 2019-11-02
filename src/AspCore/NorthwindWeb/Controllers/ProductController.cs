using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NorthwindWeb.Infrastructure.Options;
using NorthwindWeb.Models;

namespace NorthwindWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly ICategoryService _categoryService;
        private readonly ProductOptions _options;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMapper mapper, IProductService productService,
            ISupplierService supplierService, ICategoryService categoryService,
            IOptionsSnapshot<ProductOptions> options, ILogger<ProductController> logger)
        {
            _mapper = mapper;
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _options = options.Value;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Read max product count: {count}", _options.ProductCount);
            var products = _mapper.Map<IEnumerable<ProductViewModel>>(_productService.GetAll(_options.ProductCount));

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

            var model = _mapper.Map<ProductViewModel>(product);
            model.Suppliers = GetSuppliers();
            model.Categories = GetCategories();
            ViewBag.Caption = "Update product";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = GetCategories();
                model.Suppliers = GetSuppliers();

                return View(model);
            }

            var product = _mapper.Map<ProductDTO>(model);
            _productService.CreateOrUpdate(product);

            return RedirectToAction("Index");
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            var categories = _categoryService.GetAll().Select(s => new SelectListItem { Value = s.CategoryID.ToString(), Text = s.CategoryName }).ToList();
            categories.Insert(0, new SelectListItem { Value = "0", Text = "Select category" });

            return categories;
        }

        private IEnumerable<SelectListItem> GetSuppliers()
        {
            var suppliers = _supplierService.GetAll().Select(s => new SelectListItem { Value = s.SupplierID.ToString(), Text = s.CompanyName }).ToList();
            suppliers.Insert(0, new SelectListItem { Value = "0", Text = "Select supplier" });

            return suppliers;
        }
    }
}