using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using NorthwindWeb.Infrastructure.Interfaces;
using NorthwindWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure.Services
{
    public class ProductVMService : IProductVMService
    {
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;

        public ProductVMService(ICategoryService categoryService, ISupplierService supplierService)
        {
            _categoryService = categoryService;
            _supplierService = supplierService;
        }

        public void PopulateSelectLists(ProductViewModel model)
        {
            model.Categories = GetCategories();
            model.Suppliers = GetSuppliers();
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
