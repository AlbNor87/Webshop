using NUnit.Framework;
using webshop.Services.Implementations;
using webshop.Repositories.Implementations;
using webshop.Models;
using FakeItEasy;
using System.Collections.Generic;
using System;

namespace webshop.unittests.Services
{
    public class CartServiceTest
    {
        private CartService cartService;
        private ICartRepository cartRepository;

        [SetUp]

        public void SetUp()
        {
            this.cartRepository = A.Fake<ICartRepository>();
            this.cartService = new CartService(this.cartRepository);
        }

        [Test]
        public void Get_GivenValidId_ReturnsExpectedCart()
        {

            //Arrange
            const string cartId = "guid";
            var expectedCart = new List<CartItemModel> { new CartItemModel { CartId = cartId } };

            A.CallTo(() => this.cartRepository.Get(cartId)).Returns(expectedCart);

            //Act
            var result = this.cartService.Get(cartId);

            //Assert
            Assert.That(result, Is.EqualTo(expectedCart));

        }
    }
}


