using System;
using System.Text;
using NUnit.Framework;
using webshop.Services.Implementations;
using webshop.Repositories;
using webshop.Repositories.Implementations;
using webshop.Models;
using FakeItEasy;

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

        [TestCase]
        public void GetAll_ReturnsExpectedProducts()
        {
            //Arrange

            var products = new List<ProductsViewModel>
            {
                new ProductsViewModel {Id = 1}
            };

            A.CallTo(() => this.productRepository.GetAll()).Returns(products);

            //Act

            var result = this.productService.GetAll();

            //Assert

            Assert.That(result, Is.EqualTo(products));

        }
        
    }
}

namespace YRGO.CS.Project.Core.UnitTests.Services
{
    public class NewsServiceTests
    {

        private NewsService;
        private INewsRepository newsRepository;

        [SetUp]

        public void SetUp()
        {
            this.newsRepository = A.Fake<INewsRepository>();
            this.newsService = new NewsService();
        }

        [Test]
        public void GetAll_R()
        {
            //Arrange

            var news = new List<NewsModel>
            {
                new NewsModel {Id = 1337}
            }

            A.CallTo(() => this.newsRepository.GetAll()).Returns(news);

            //Act

            var result = this.newsService.GetAll();

            //Assert

            Assert.That(result, Is.EqualTo(news))

        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Get_GivenIdLessThanOne_ReturnsNull(int id)
        {

            //Act


            var result = this.newsService.Get(id);

            //Assert

            Assert.That(result, Is.Null);

        }

        [TestCase]
        public void Get_GivenValidId_ReturnsExpectedNews(int id)
        {

            //Arrange

            const int Id = 13;
            var expectedNewsItem = new NewsModel { Id = Id };

            A.CallTo(() => this.newsRepository.Get(Id).Returns(expectedNewsItem));

            //Act

            var result = this.newsService.Get(Id);

            //Assert

            Assert.That(result, Is.EqualTo(expectedNewsItem));

        }

        [TestCase("kebab", "kebab är gött!")]
        [TestCase("kebab", null)]
        [TestCase("kebab", "")]
        [TestCase(null, "kebab är gött!")]
        [TestCase("", "kebab är gött!")]
        [TestCase("", "")]
        [TestCase(null, null)]
        public void Create_GivenNews_ReturnsExpected(string header, string body)
        {

            //Arrange

            var news = new NewsModel { Header = header, Body = body };

            //Act

            var result = this.newsService.Create(news);

            //Assert

            Assert.That(result, Is.False);
            A.CallTo(() => this.newsRepository.Create(A<NewsModel>._)).MustNotHaveHappened();

        }


    }
}
