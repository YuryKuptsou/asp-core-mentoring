using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindWeb.Infrastructure.Extensions;
using NorthwindWeb.Models.Api;

namespace NorthwindWeb.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryApiController(ICategoryService categoryService)
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

        [HttpGet("category/image/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult GetImage(int id)
        {
            var category = _categoryService.Get(id);
            if (category == null)
            {
                return BadRequest();
            }

            return File(category.Picture, category.ContentType);
        }

        [HttpPost("category/image/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult UpdateImage(int id,[FromForm] ImageApiModel image)
        {
            var category = _categoryService.Get(id);
            if (category == null || image.File == null || !image.File.IsImage())
            {
                return BadRequest();
            }

            var stream = new MemoryStream();
            image.File.CopyTo(stream);
            category.Picture = stream.ToArray();
            category.ContentType = image.File.ContentType;
            _categoryService.Update(category);

            return Ok();
        }
    }
}