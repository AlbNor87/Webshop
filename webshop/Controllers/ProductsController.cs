using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webshop.Models;
using webshop.Services.Implementations;
using webshop.Repositories.Implementations;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;




// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webshop.Controllers
{
    public class ProductsController : Controller
    {

        private readonly ProductService productService;

        public ProductsController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            this.productService = new ProductService(
                new ProductRepository(connectionString));
        }


        public IActionResult Index()
        {

            var products = this.productService.GetAll();
            return View(products);

        }

    }
}
