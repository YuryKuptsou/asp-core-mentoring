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
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductApiController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("product/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult Get(int id)
        {
            var product = _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("products")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDTO>))]
        public ActionResult<ProductDTO> GetAll()
        {
            var products = _productService.GetAll();

            return Ok(products);
        }

        [HttpPost("product")]
        [ProducesResponseType(201)]
        public ActionResult Create([FromBody] ProductDTO product)
        {
            product.ProductID = 0;
            _productService.CreateOrUpdate(product);

            return CreatedAtAction("Get", new { id = product.ProductID }, product.ProductID);
        }

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