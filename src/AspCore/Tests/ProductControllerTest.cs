using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NorthwindWeb.Controllers;
using NorthwindWeb.Infrastructure.Options;
using NorthwindWeb.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class ProductControllerTest
    {
        private ProductDTO _product;

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOptionsSnapshot<ProductOptions> _options;
        private readonly ILogger<ProductController> _logger;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public ProductControllerTest()
        {
            _mapper = Mock.Of<IMapper>();
            Mock.Get(_mapper).Setup(m => m.Map<IEnumerable<ProductViewModel>>(It.IsAny<object>())).Returns(GetProducts());
            Mock.Get(_mapper).Setup(m => m.Map<ProductViewModel>(It.IsAny<object>())).Returns(new ProductViewModel { ProductID = 1 });

            _productService = Mock.Of<IProductService>();
            Mock.Get(_productService).Setup(s => s.GetAll(It.IsAny<int>())).Returns(new[] {new ProductDTO()});
            Mock.Get(_productService).Setup(s => s.Get(It.IsAny<int>())).Returns(() => _product);

            _supplierService = Mock.Of<ISupplierService>();
            Mock.Get(_supplierService).Setup(s => s.GetAll()).Returns(new[] { new SupplierDTO() });

            _categoryService = Mock.Of<ICategoryService>();
            Mock.Get(_categoryService).Setup(s => s.GetAll()).Returns(new[] { new CategoryDTO() });

            _options = Mock.Of<IOptionsSnapshot<ProductOptions>>(m => m.Value == new ProductOptions { ProductCount = 3 });
            _logger = Mock.Of<ILogger<ProductController>>();
        }



        [Test]
        public void Index_ReturnViewResult_WithProductList()
        {
            //Arrange
            var controller = new ProductController(_mapper, _productService, null, null, _options, _logger);

            //Act
            var result = controller.Index();

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.AssignableTo<IEnumerable<ProductViewModel>>());
            var model = viewResult.ViewData.Model as IEnumerable<ProductViewModel>;
            Assert.That(model, Has.Exactly(GetProducts().Count()).Items);
        }

        [Test]
        public void Create_ReturnViewResult_WithZeroModelId()
        {
            //Arrange
            var controller = new ProductController(_mapper, null, _supplierService, _categoryService, _options, _logger);

            //Act
            var result = controller.Create();

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewName, Is.EqualTo("Update"));
            Assert.That(viewResult.ViewData.Model, Is.InstanceOf<ProductViewModel>());
            var model = viewResult.ViewData.Model as ProductViewModel;
            Assert.That(model.ProductID, Is.Zero);
        }

        [Test]
        public void Update_ReturnViewresult_WithNoneZeroId()
        {
            //Arrange
            _product = new ProductDTO { ProductID = 1 };
            var controller = new ProductController(_mapper, _productService, _supplierService, _categoryService, _options, _logger);

            //Act
            var result = controller.Update(id: 1);

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.InstanceOf<ProductViewModel>());
            var model = viewResult.ViewData.Model as ProductViewModel;
            Assert.That(model.ProductID, Is.Not.Zero);
        }

        [Test]
        public void Update_ReturnNoContentResult_WithInvalidId()
        {
            //Arrange
            _product = null;
            var controller = new ProductController(_mapper, _productService, _supplierService, _categoryService, _options, _logger);

            //Act
            var result = controller.Update(id: -1);

            //Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public void UpdatePost_ReturnViewResult_WhenModelStateIsNotValid()
        {
            //Arrange
            var controller = new ProductController(_mapper, null, _supplierService, _categoryService, _options, _logger);
            controller.ModelState.AddModelError("ProductName", "Required");
            var model = new ProductViewModel();

            //Act
            var result = controller.Update(model);

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.InstanceOf<ProductViewModel>());
            Assert.That(viewResult.ViewData.ModelState.ErrorCount, Is.Not.Zero);
        }

        [Test]
        public void UpdatePost_ReturnRedirectToActionAndCreateOrUpdate_WhenModelIsValid()
        {
            //Arrange
            Mock.Get(_productService).Setup(s => s.CreateOrUpdate(It.IsAny<ProductDTO>())).Verifiable();
            var controller = new ProductController(_mapper, _productService, _supplierService, _categoryService, _options, _logger);
            var model = new ProductViewModel { ProductID = 0 };

            //Act
            var result = controller.Update(model);

            //Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult.ControllerName, Is.Null);
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Mock.Get(_productService).Verify();
        }

        private IEnumerable<ProductViewModel> GetProducts()
        {
            return new List<ProductViewModel>
            {
                new ProductViewModel(),
                new ProductViewModel()
            };
        }

        private IEnumerable<SupplierDTO> GetSuppliers()
        {
            return new List<SupplierDTO>
            {
                new SupplierDTO(),
                new SupplierDTO()
            };
        }

        private IEnumerable<CategoryDTO> GetCategories()
        {
            return new List<CategoryDTO>
            {
                new CategoryDTO(),
                new CategoryDTO()
            };
        }
    }
}
