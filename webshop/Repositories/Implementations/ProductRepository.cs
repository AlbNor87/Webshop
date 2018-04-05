using System;
using System.Collections.Generic;
using webshop.Models;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;

namespace webshop.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {

        private readonly string connectionString;

        public ProductRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public List<ProductsViewModel> GetAll()
        {
            
            using (var connection = new MySqlConnection(this.connectionString))
            {
                return connection.Query<ProductsViewModel>("Select * from Products").ToList();
            }

        }

    }
}
