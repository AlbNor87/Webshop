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
    public class CartController : Controller
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
                    var carts = connection.Query<CartItemModel>("SELECT carts.cartId, sum(carts.quantity) as quantity, carts.productId, products.price, products.brand, products.model, products.image FROM products INNER JOIN carts ON carts.productId = products.id WHERE carts.cartId = @cartId GROUP BY carts.productId;",
                        new { cartId }).ToList();
                    return View(carts);

                }
                catch (Exception)
                {
                    return NotFound();
                }

            }

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

            using (var connection = new MySqlConnection(this.connectionString))
            {

                try
                {
                    connection.Execute(
                        "INSERT INTO Carts (productId, cartId, quantity, dateCreated) VALUES(@id, @cartId, @quantity, NOW())",
                        new { id, cartId, quantity});
                }
                catch (Exception)
                {
                    return NotFound();
                }

            }


            return RedirectToAction("Index", "Products");

        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {

            var cartId = GetOrCreateCartId();

            using (var connection = new MySqlConnection(this.connectionString))
            {

                try
                {   
                    
                    connection.Execute(
                        "DELETE FROM Carts WHERE carts.cartId = @cartId AND carts.productId = @id;",
                        new { id, cartId});
                }
                catch (Exception)
                {
                    //return NotFound();
                }

            }

            return RedirectToAction("Index", "Cart");

        }


        public CartController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("ConnectionString");
        }

    }
}
