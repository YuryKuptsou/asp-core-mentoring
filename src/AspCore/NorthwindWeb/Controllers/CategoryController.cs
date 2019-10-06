using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NorthwindWeb.Infrastructure.Interfaces;
using NorthwindWeb.Models;

namespace NorthwindWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = _categoryService.GetAll()
                .Select(s => new CategoryViewModel { CategoryName = s.CategoryName });

            return View(categories);
        }
    }
}