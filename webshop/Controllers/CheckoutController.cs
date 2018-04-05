using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;
using webshop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webshop.Controllers
{
    public class CheckoutController : Controller
    {

        public string CartId { get; set; }

        private readonly string connectionString;

        // GET: /<controller>/
        public IActionResult Index()
        {

            var cartId = GetOrCreateCartId();

            using (var connection = new MySqlConnection(this.connectionString))
            {
                try
                {
                    var checkoutItems = connection.Query<CartItemModel>("SELECT carts.cartId, sum(carts.quantity) as quantity, carts.productId, products.price, products.brand, products.model, products.image FROM products INNER JOIN carts ON carts.productId = products.id WHERE carts.cartId = @cartId GROUP BY carts.productId;",
                    new { cartId }).ToList();

                    var sum = checkoutItems.Select(c => c.Price * c.Quantity).Sum();

                    var checkoutItem = new CheckoutItemModel();
                    checkoutItem.Cart = checkoutItems;
                    checkoutItem.Sum = sum;

                    return View(checkoutItem);

                }
                catch (Exception)
                {
                    return NotFound();
                }

            }

        }

        [HttpPost]
        public ActionResult PlaceOrder(string firstname, string lastname, string email, string adress, int zipcode, string payment, string cartId, int sum)
        {

            using (var connection = new MySqlConnection(this.connectionString))
            {

                try
                {
                    
                    connection.Execute(
                        "INSERT INTO Orders(firstName, lastName, email, adress, zipCode, paymentMethod, sum, cartId) VALUES(@firstname, @lastname, @email, @adress, @zipcode, @payment, @sum, @cartId)",
                        new { firstname, lastname, email, adress, zipcode, payment, sum, cartId });


                    var orderInfo = connection.Query<OrderInfoModel>("Select * from Orders where orders.cartId = @cartId;", 
                                                                 new { cartId }).ToList();


                    var checkoutItems = connection.Query<CartItemModel>(
                        "SELECT carts.cartId, sum(carts.quantity) as quantity, carts.productId, products.price, products.brand, products.model, products.image FROM products INNER JOIN carts ON carts.productId = products.id WHERE carts.cartId = @cartId GROUP BY carts.productId;",
                        new { cartId }).ToList();

                    var orderId = orderInfo[0].Id;

                    foreach(var item in checkoutItems)
                    {

                        connection.Execute(
                            "INSERT INTO OrderRows(orderId, productId, brand, model, quantity, price) VALUES(@orderId, @productId, @brand, @model, @quantity, @price)",
                            new {orderId, item.ProductId, item.Brand, item.Model, item.Quantity, item.Price});
                    }

                }
                catch (Exception)
                {
                    return NotFound();
                }

            }


            return RedirectToAction("Index", "Products");

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

        public CheckoutController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("ConnectionString");
        }

    }
}
