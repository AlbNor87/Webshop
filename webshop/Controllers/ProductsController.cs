using Microsoft.AspNetCore.Mvc;
using webshop.Services.Implementations;
using webshop.Repositories.Implementations;
using Microsoft.Extensions.Configuration;

namespace webshop.Controllers
{
    public class ProductsController : Controller
    {

        private readonly ProductService productService;

        public ProductsController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");

            this.productService = new ProductService(new ProductRepository(connectionString));
        }


        public IActionResult Index()
        { 

            var products = this.productService.GetAll();

            return View(products);

        }

    }
}
