using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NorthwindWeb.Models;

namespace NorthwindWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMapper mapper ,ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = _mapper.Map<IEnumerable<CategoryViewModel>>(_categoryService.GetAll());

            return View(categories);
        }
    }
}