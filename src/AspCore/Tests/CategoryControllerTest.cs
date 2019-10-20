using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
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
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryControllerTest()
        {
            _categoryService = Mock.Of<ICategoryService>();
            Mock.Get(_categoryService).Setup(c => c.GetAll()).Returns(new[] {new CategoryDTO()});

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
