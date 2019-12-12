using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindWeb.Infrastructure.Extensions;
using NorthwindWeb.Models.Api;

namespace NorthwindWeb.Controllers.Api
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryApiController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Returns category by id.
        /// </summary>
        /// <param name="id">
        /// Category id.
        /// </param>
        /// <remarks>The endpoint to get category.</remarks>
        /// <returns>Returns product.</returns>
        /// <response code="200">Returns category.</response>
        /// <response code="404">Category is not found.</response>
        [AllowAnonymous]
        [HttpGet("category/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<CategoryDTO> Get(int id)
        {
            var category = _categoryService.Get(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Returns all categories.
        /// </summary>
        /// <remarks>The endpoint to get all categories.</remarks>
        /// <returns>Returns all categories.</returns>
        /// <response code="200">Returns all categories.</response>
        [AllowAnonymous]
        [HttpGet("categories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDTO>))]
        public ActionResult<CategoryDTO> GetAll()
        {
            var categories = _categoryService.GetAll();

            return Ok(categories);
        }


        /// <summary>
        /// Returns category image by id.
        /// </summary>
        /// <param name="id">
        /// Category id.
        /// </param>
        /// <remarks>The endpoint to get category image.</remarks>
        /// <returns>Returns category image.</returns>
        /// <response code="200">Returns category image.</response>
        /// <response code="404">Category is not found.</response>
        [AllowAnonymous]
        [HttpGet("category/image/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult GetImage(int id)
        {
            var category = _categoryService.Get(id);
            if (category == null)
            {
                return NotFound();
            }

            return File(category.Picture, category.ContentType);
        }

        /// <summary>
        /// Uploads category image by id.
        /// </summary>
        /// <param name="id">
        /// Category id.
        /// </param>
        /// <param name="image">
        /// Image.
        /// </param>
        /// <remarks>The endpoint to upload category image.</remarks>
        /// <response code="200">Image is uploaded.</response>
        /// <response code="400">Wrong image.</response>
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