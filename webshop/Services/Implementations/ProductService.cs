using System;
using System.Collections.Generic;
using webshop.Models;
using webshop.Repositories.Implementations;
using webshop.Repositories;
using System.Web;


namespace webshop.Services.Implementations
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public List<ProductsViewModel> GetAll()
        {
            return this.productRepository.GetAll();
        }      
    }
}
