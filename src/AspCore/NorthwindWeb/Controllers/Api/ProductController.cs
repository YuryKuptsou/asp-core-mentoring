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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
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
    }
}