using DAL;
using DAL.Domains;
using DAL.Repositories;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiTests
{
    [TestFixture]
    public class ProductApiTest
    {
        private readonly string _apiBase = "http://localhost:5000/";
        private readonly Repository<Product> _productRepository;
        private readonly ProductDTO _product;
        private ProductApiApi _instance;
        private int? _id; 


        public ProductApiTest()
        {
            var context = new NorthwindContext("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True");
            _productRepository = new Repository<Product>(context);

            _product = new ProductDTO
            {
                ProductID = 0,
                ProductName = "Test product",
                QuantityPerUnit = "quantity",
                UnitPrice = 0,
                UnitsInStock = 0,
                UnitsOnOrder = 0,
                ReorderLevel = 0,
                Discontinued = true,
                SupplierID = 1,
                CategoryID = 1,
            };
        }

        [SetUp]
        public void Init()
        {
            _instance = new ProductApiApi(_apiBase);
        }

        [TearDown]
        public void Cleanup()
        {
            if (_id.HasValue)
            {
                var product = _productRepository.Get(_id.Value);
                if (product != null)
                {
                    _productRepository.Delete(product);
                }
            }
        }


        [Test]
        public void Create_ValidProduct_Created()
        {
            //Arrange
            int before = _productRepository.GetAll().Count();

            //Act
            _id = _instance.Create(_product);
            int after = _productRepository.GetAll().Count();

            //Assert
            Assert.AreEqual(before + 1, after);
        }

        [Test]
        public void Remove_Removed()
        {
            //Arrange
            _id = _instance.Create(_product);
            int before = _productRepository.GetAll().Count();

            //Act
            _instance.Remove(_id);
            int after = _productRepository.GetAll().Count();

            //Assert
            Assert.AreEqual(before - 1, after);
        }


        [Test]
        public void Update_Updated()
        {
            //Arrange
            var id = _instance.Create(_product);
            ProductDTO product = new ProductDTO
            {
                ProductID = id,
                ProductName = "new product",
                QuantityPerUnit = "new quantity",
                UnitPrice = 1,
                UnitsInStock = 1,
                UnitsOnOrder = 1,
                ReorderLevel = 1,
                Discontinued = false,
                SupplierID = 2,
                CategoryID = 2,
            };

            //Act
            _instance.Update(id, product);
            var updatedProduct = _productRepository.Get(id.Value);

            //Assert
            Assert.That(updatedProduct, Has.Property(nameof(ProductDTO.ProductName)).EqualTo(product.ProductName));
            Assert.That(updatedProduct, Has.Property(nameof(ProductDTO.QuantityPerUnit)).EqualTo(product.QuantityPerUnit));
            Assert.That(updatedProduct, Has.Property(nameof(ProductDTO.UnitPrice)).EqualTo(product.UnitPrice));
            Assert.That(updatedProduct, Has.Property(nameof(ProductDTO.UnitsInStock)).EqualTo(product.UnitsInStock));
            Assert.That(updatedProduct, Has.Property(nameof(ProductDTO.UnitsOnOrder)).EqualTo(product.UnitsOnOrder));
            Assert.That(updatedProduct, Has.Property(nameof(ProductDTO.ReorderLevel)).EqualTo(product.ReorderLevel));
            Assert.That(updatedProduct, Has.Property(nameof(ProductDTO.Discontinued)).EqualTo(product.Discontinued));
            Assert.That(updatedProduct, Has.Property(nameof(ProductDTO.SupplierID)).EqualTo(product.SupplierID));
            Assert.That(updatedProduct, Has.Property(nameof(ProductDTO.CategoryID)).EqualTo(product.CategoryID));

        }

        [Test]
        public void Update_InvalidID_BadRequest()
        {
            //Arrange
            int? id = -100;
            ProductDTO product = new ProductDTO
            {
                ProductID = id,
                ProductName = "new product",
                QuantityPerUnit = "new quantity",
                UnitPrice = 1,
                UnitsInStock = 1,
                UnitsOnOrder = 1,
                ReorderLevel = 1,
                Discontinued = false,
                SupplierID = 2,
                CategoryID = 2,
            };

            //Act


            //Assert
            var error = Assert.Throws<ApiException>(() => _instance.Update(id, product));
            Assert.AreEqual(error.ErrorCode, StatusCodes.Status400BadRequest);
        }

    }
}
