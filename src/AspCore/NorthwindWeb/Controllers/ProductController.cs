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
using NorthwindWeb.Infrastructure.Interfaces;
using NorthwindWeb.Infrastructure.Options;
using NorthwindWeb.Models;
using SmartBreadcrumbs.Attributes;

namespace NorthwindWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IProductVMService _productVMService;
        private readonly ProductOptions _options;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMapper mapper, IProductService productService,
            IProductVMService productVMService, IOptionsSnapshot<ProductOptions> options,
            ILogger<ProductController> logger)
        {
            _mapper = mapper;
            _productService = productService;
            _productVMService = productVMService;
            _options = options.Value;
            _logger = logger;
        }

        [Breadcrumb("Products")]
        public IActionResult Index()
        {
            _logger.LogInformation("Read max product count: {count}", _options.ProductCount);
            var products = _mapper.Map<IEnumerable<ProductViewModel>>(_productService.GetAll(_options.ProductCount));

            return View(products);
        }

        [Breadcrumb("Create")]
        public IActionResult Create()
        {
            var model = new ProductViewModel();
            _productVMService.PopulateSelectLists(model);

            ViewBag.Caption = "Create product";

            return View("Update", model);
        }

        [Breadcrumb("Update")]
        public IActionResult Update(int id)
        {
            var product = _productService.Get(id);
            if (product == null)
            {
                return NoContent();
            }

            var model = _mapper.Map<ProductViewModel>(product);
            _productVMService.PopulateSelectLists(model);
            ViewBag.Caption = "Update product";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _productVMService.PopulateSelectLists(model);

                return View(model);
            }

            var product = _mapper.Map<ProductDTO>(model);
            _productService.CreateOrUpdate(product);

            return RedirectToAction("Index");
        }
    }
}