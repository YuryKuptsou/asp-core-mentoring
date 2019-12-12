using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindWeb.Controllers.Api
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductApiController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Returns product by id.
        /// </summary>
        /// <param name="id">
        /// Product id.
        /// </param>
        /// <remarks>The endpoint to get product.</remarks>
        /// <returns>Returns product.</returns>
        /// <response code="200">Returns product.</response>
        /// <response code="404">Product is not found.</response>
        [AllowAnonymous]
        [HttpGet("product/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ProductDTO> Get(int id)
        {
            var product = _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Returns all products.
        /// </summary>
        /// <remarks>The endpoint to get all products.</remarks>
        /// <returns>Returns all products.</returns>
        /// <response code="200">Returns all products.</response>
        [AllowAnonymous]
        [HttpGet("products")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDTO>))]
        public ActionResult<ProductDTO> GetAll()
        {
            var products = _productService.GetAll();

            return Ok(products);
        }

        /// <summary>
        /// Adds product.
        /// </summary>
        /// <remarks>The endpoint to add product.</remarks>
        /// <returns>Returns product id.</returns>
        /// <response code="201">Returns product id.</response>
        [HttpPost("product")]
        [ProducesResponseType(201)]
        public ActionResult<int> Create([FromBody] ProductDTO product)
        {
            product.ProductID = 0;
            var id = _productService.CreateOrUpdate(product);

            return CreatedAtAction("Get", new { id }, id);
        }

        /// <summary>
        /// Updates product.
        /// </summary>
        /// <param name="id">
        /// Product id.
        /// </param>
        /// <param name="product">
        /// Product info.
        /// </param>
        /// <remarks>The endpoint to update product.</remarks>
        /// <response code="200">Product is updated.</response>
        /// <response code="400">Wrong product info.</response>
        [HttpPut("product/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult Update(int id, [FromBody] ProductDTO product)
        {
            var _product = _productService.Get(id);
            if (_product == null)
            {
                return BadRequest();
            }

            _productService.CreateOrUpdate(product);

            return Ok();
        }


        /// <summary>
        /// Removes product.
        /// </summary>
        /// <remarks>The endpoint to remove product.</remarks>
        /// <response code="204">Product is removed.</response>
        [HttpDelete("product/{id}")]
        [ProducesResponseType(204)]
        public ActionResult Remove(int id)
        {
            var product = _productService.Get(id);
            if (product != null)
            {
                _productService.Remove(product);
            }

            return NoContent();
        }
    }
}