using System;
using System.Collections.Generic;
using webshop.Models;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;

namespace webshop.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {

        private readonly string connectionString;

        public CartRepository(string connectionString)
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

        public bool AddToCart(int id, string cartId, int quantity)
        {
            try
            {
                using (var connection = new MySqlConnection(this.connectionString))
                {

                    var itemExists = connection.QuerySingleOrDefault("SELECT * FROM carts WHERE productId = @id AND cartId = @cartId;", new { id, cartId });

                    if (itemExists != null)
                    {
                        connection.Execute(
                            "UPDATE carts SET quantity = quantity + @quantity WHERE productId = @id AND cartId = @cartId;",
                            new { id, cartId, quantity });

                        return true;
                    }

                    connection.Execute(
                        "INSERT INTO Carts (productId, cartId, quantity, dateCreated) VALUES(@id, @cartId, @quantity, NOW())",
                        new { id, cartId, quantity });

                    return true;
                }

            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
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
