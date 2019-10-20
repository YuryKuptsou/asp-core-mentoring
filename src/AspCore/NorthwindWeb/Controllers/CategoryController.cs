﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NorthwindWeb.Infrastructure.Extension;
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

        public IActionResult Image(int id)
        {
            var category = _categoryService.Get(id);
            if (category == null)
            {
                return NoContent();
            }

            return new FileStreamResult(new MemoryStream(category.Picture), category.ContentType);
        }

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