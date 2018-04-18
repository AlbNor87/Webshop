using NUnit.Framework;
using webshop.Services.Implementations;
using webshop.Repositories.Implementations;
using webshop.Models;
using FakeItEasy;
using System.Collections.Generic;

namespace webshop.unittests.Services
{
    public class ProductServiceTest
    {
        private ProductService productService;
        private IProductRepository productRepository;

        [SetUp]

        public void SetUp()
        {
            this.productRepository = A.Fake<IProductRepository>();
            this.productService = new ProductService(this.productRepository);
        }

        [Test]
        public void GetAll_ReturnsExpectedProducts()
        {
            //Arrange

            var products = new List<ProductsViewModel>
            {
                new ProductsViewModel()
            };

            A.CallTo(() => this.productRepository.GetAll()).Returns(products);

            //Act

            var result = this.productService.GetAll();

            //Assert

            Assert.That(result, Is.EqualTo(products));
        }
    }
}


