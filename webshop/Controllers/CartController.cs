﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using webshop.Repositories.Implementations;
using webshop.Services.Implementations;

namespace webshop.Controllers
{
    public class CartController : Controller
    {

        private readonly CartService cartService;

        public string CartId { get; set; }

        public CartController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            this.cartService = new CartService(
                new CartRepository(connectionString));
        }


        public IActionResult Index()
        {

            var cartId = GetOrCreateCartId();

            var cartItems = this.cartService.Get(cartId);

            return View(cartItems);

        }


        private string GetOrCreateCartId()
        {
            if (this.Request.Cookies.ContainsKey("CartId"))
            {
                return this.Request.Cookies["CartId"];
            }

            var cartId = Guid.NewGuid().ToString();

            this.Response.Cookies.Append("CartId", cartId);

            return cartId;
        }


        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {

            var cartId = GetOrCreateCartId();

            this.cartService.AddToCart(id, cartId, quantity);

            return RedirectToAction("Index", "Products");

        }


        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {

            var cartId = GetOrCreateCartId();

            this.cartService.RemoveFromCart(id, cartId);

            return RedirectToAction("Index", "Cart");

        }
    }
}
