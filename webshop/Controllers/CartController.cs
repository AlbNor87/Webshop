using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;
using webshop.Models;
using webshop.Repositories.Implementations;
using webshop.Services.Implementations;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public ActionResult AddToCart(int id)
        {

            var cartId = GetOrCreateCartId();
            var quantity = 1;

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
