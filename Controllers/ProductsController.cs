using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webshop.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webshop.Controllers
{
    public class ProductsController : Controller
    {

        private readonly string connectionString;

        // GET: /<controller>/
        public IActionResult Index()
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                var products = connection.Query<ProductsViewModel>("Select * from Products").ToList();
                return View(products);
            }

            //return ;
        }

        public IActionResult Get(string id)
        {

            using (var connection = new MySqlConnection(this.connectionString))
            {
                var productItem = connection.QuerySingleOrDefault<ProductsViewModel>("select * from Products where id = @id", new { id });
                if (productItem != null)
                {
                    return View(productItem);
                }
                else
                {
                    return NotFound();
                }
            }

        }

        [HttpGet]
        // GET: /<controller>/
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductsViewModel model)
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                connection.Execute(
                    "INSERT INTO Products (brand, price, model) VALUES(@Brand, @Price, @Model)",
                    new { model.Brand, model.Price, model.Model });
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {

            using (var connection = new MySqlConnection(this.connectionString))
            {
                var productItem = connection.QuerySingleOrDefault<ProductsViewModel>("select * from Products where id = @id", new { id });
                if (productItem != null)
                {
                    return View(productItem);
                }
                else
                {
                    return NotFound();
                }
            }

        }

        [HttpPost]
        public ActionResult Edit(ProductsViewModel model)
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                connection.Execute(
                    "UPDATE Products SET brand = @Brand, price = @Price, model = @Model WHERE id = @id", new { model.Brand, model.Price, model.Model, model.Id });
            }

            return RedirectToAction("Index");
        }

        public ProductsController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("ConnectionString");
        }



    }
}
