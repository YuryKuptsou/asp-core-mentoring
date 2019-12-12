using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindWeb.Infrastructure.Extensions;
using NorthwindWeb.Models;
using SmartBreadcrumbs.Attributes;

namespace NorthwindWeb.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMapper mapper ,ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [Breadcrumb("Categories")]
        public IActionResult Index()
        {
            var categories = _mapper.Map<IEnumerable<CategoryViewModel>>(_categoryService.GetAll());

            return View(categories);
        }

        public IActionResult Image(int id)
        {
            var category = _categoryService.Get(id);
            if (category == null)
            {
                return NoContent();
            }

            return new FileStreamResult(new MemoryStream(category.Picture), category.ContentType);
        }

        [Breadcrumb("Update")]
        public IActionResult Update(int id)
        {
            var category = _categoryService.Get(id);
            if (category == null)
            {
                return NoContent();
            }
            var model = new CategoryViewModel
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName
            };
            ViewBag.Caption = "Upload new image";

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(CategoryViewModel model)
        {
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Use jpeg, png, gif image formats");

                return View(model);
            }

            _categoryService.Update(_mapper.Map<CategoryDTO>(model));

            return RedirectToAction("Index");
        }
    }
}