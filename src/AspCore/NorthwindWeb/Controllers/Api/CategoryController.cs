using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindWeb.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet("category/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult Get(int id)
        {
            var category = _categoryService.Get(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("categories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDTO>))]
        public ActionResult<CategoryDTO> GetAll()
        {
            var categories = _categoryService.GetAll();

            return Ok(categories);
        }
    }
}