using Microsoft.AspNetCore.Mvc;
using NorthwindWeb.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Index_ReturnViewResult()
        {
            //Arrange
            var controller = new HomeController();

            //Act
            var result = controller.Index();

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

    }
}
