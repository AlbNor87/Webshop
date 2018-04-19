using System;
using System.Text;
using System.Collections.Generic;
using webshop.Models;


namespace webshop.Repositories.Implementations
{
    public interface IProductRepository
    {
        List<ProductsViewModel> GetAll();
    }
}

 