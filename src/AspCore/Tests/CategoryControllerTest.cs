using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NorthwindWeb.Controllers;
using NorthwindWeb.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class CategoryControllerTest
    {
        private CategoryDTO _category;

        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryControllerTest()
        {
            _categoryService = Mock.Of<ICategoryService>();
            Mock.Get(_categoryService).Setup(c => c.GetAll()).Returns(new[] {new CategoryDTO()});
            Mock.Get(_categoryService).Setup(s => s.Get(It.IsAny<int>())).Returns(() => _category);

            _mapper = Mock.Of<IMapper>();
            Mock.Get(_mapper).Setup(m => m.Map<IEnumerable<CategoryViewModel>>(It.IsAny<object>())).Returns(GetCatigories());
        }

        [Test]
        public void Index_ReturnViewResult_WithCategoryList()
        {
            //Arrange
            var controller = new CategoryController(_mapper, _categoryService);

            //Act
            var result = controller.Index();

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.AssignableTo<IEnumerable<CategoryViewModel>>());
            var model = viewResult.ViewData.Model as IEnumerable<CategoryViewModel>;
            Assert.That(model, Has.Exactly(GetCatigories().Count()).Items);

        }

        [Test]
        public void Image_ReturnNoContentResult_WithInvalidId()
        {
            //Arrange
            var controller = new CategoryController(_mapper, _categoryService);
            _category = null;

            //Act
            var result = controller.Image(id: -1);

            //Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public void Image_ReturnFileStreamResult_WithValidId()
        {
            //Arrange
            var controller = new CategoryController(_mapper, _categoryService);
            _category = new CategoryDTO { ContentType = "image/jpg", Picture = new byte[] { 1 } };

            //Act
            var result = controller.Image(id: 1);

            //Assert
            Assert.That(result, Is.InstanceOf<FileStreamResult>());
        }

        [Test]
        public void UpdatePost_ReturnViewResult_WhenFileIsNotImage()
        {
            //Arrange
            var fromFile = Mock.Of<IFormFile>();
            Mock.Get(fromFile).Setup(f => f.ContentType).Returns("notimage");
            var model = new CategoryViewModel { Image = fromFile };
            var controller = new CategoryController(_mapper, _categoryService);

            //Act
            var result = controller.Update(model);

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.ModelState.ErrorCount, Is.Not.Zero);
        }

        [Test]
        public void UpdatePost_ReturnRedirectToAction_WhenFileIsImage()
        {
            //Arrange
            var fromFile = Mock.Of<IFormFile>();
            Mock.Get(fromFile).Setup(f => f.ContentType).Returns("image/jpg");
            Mock.Get(fromFile).Setup(f => f.FileName).Returns("1.jpg");
            Mock.Get(fromFile).Setup(f => f.Length).Returns(1000);
            Mock.Get(_categoryService).Setup(c => c.Update(It.IsAny<CategoryDTO>())).Verifiable();
            var model = new CategoryViewModel { Image = fromFile };
            var controller = new CategoryController(_mapper, _categoryService);

            //Act
            var result = controller.Update(model);

            //Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Mock.Get(_categoryService).Verify();
        }

        private IEnumerable<CategoryViewModel> GetCatigories()
        {
            return new List<CategoryViewModel>
            {
                new CategoryViewModel { CategoryName = "cat1" },
                new CategoryViewModel { CategoryName = "cat2" }
            };
        }

    }
}
