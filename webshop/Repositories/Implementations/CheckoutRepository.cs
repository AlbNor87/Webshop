using System;
using System.Collections.Generic;
using webshop.Models;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using webshop.Repositories.Implementations;
using webshop.Services.Implementations;


namespace webshop.Repositories.Implementations
{
    public class CheckoutRepository : ICheckoutRepository
    {

        private readonly string connectionString;

        public CheckoutRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<CartItemModel> Get(string cartId)
        {
            
            using (var connection = new MySqlConnection(this.connectionString))
            {
                    return connection.Query<CartItemModel>("SELECT carts.cartId, sum(carts.quantity) as quantity, carts.productId, products.price, products.brand, products.model, products.image FROM products INNER JOIN carts ON carts.productId = products.id WHERE carts.cartId = @cartId GROUP BY carts.productId;",
                    new { cartId }).ToList();
            }

        }


        public bool PlaceOrder(string firstname, string lastname, string email, string adress, int zipcode, string payment, string cartId, int sum)
        {

            try
            {

                using (var connection = new MySqlConnection(this.connectionString))
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

                    foreach (var item in checkoutItems)
                    {

                        connection.Execute(
                            "INSERT INTO OrderRows(orderId, productId, brand, model, quantity, price) VALUES(@orderId, @productId, @brand, @model, @quantity, @price)",
                            new { orderId, item.ProductId, item.Brand, item.Model, item.Quantity, item.Price });
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public void RemoveFromCart(int id, string cartId)
        {

            using (var connection = new MySqlConnection(this.connectionString))
            {

                    connection.Execute(
                        "DELETE FROM Carts WHERE carts.cartId = @cartId AND carts.productId = @id;",
                        new { id, cartId });
            }
        }
    }
}
