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

namespace webshop.Controllers
{
    public class CheckoutController : Controller
    {

        public string CartId { get; set; }

        private readonly CheckoutService checkoutService;

        public CheckoutController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            this.checkoutService = new CheckoutService(
                new CheckoutRepository(connectionString));
        }


        public IActionResult Index()
        {
            
            var cartId = GetOrCreateCartId();

            var cartItems = this.checkoutService.Get(cartId);

            if (this.Request.QueryString.Value.Contains("error") && cartItems != null)
            {
                cartItems.Message = "Something went wrong. Your order did not go through. Sorry!";
            }

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult PlaceOrder(string firstname, string lastname, string email, string adress, int zipcode, string payment, int sum)
        {
            var cartId = GetOrCreateCartId();

            var orderResult = this.checkoutService.PlaceOrder(firstname, lastname, email, adress, zipcode, payment, cartId, sum);

            if(!orderResult.Success)
            {
                return RedirectToAction("Index", new { error = true });
            }

            this.Response.Cookies.Delete("CartId");
            return RedirectToAction("Receipt", new { orderResult.OrderId });
        }

        public IActionResult Receipt(int orderId)
        {
            var receipt = this.checkoutService.GetOrderInfo(orderId);

            return View(receipt);
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
    }
}
