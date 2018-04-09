using System;
using System.Collections.Generic;
using webshop.Models;
using webshop.Repositories.Implementations;
using webshop.Repositories;
using System.Web;


namespace webshop.Services.Implementations
{
    public class CartService
    {
        private readonly ICartRepository cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }


        public List<CartItemModel> Get(string cartId)
        {
            return this.cartRepository.Get(cartId);
        }


        public void AddToCart(int id, string cartId, int quantity)
        {
            this.cartRepository.AddToCart(id, cartId, quantity);
        }


        public void RemoveFromCart(int id, string cartId)
        {
            this.cartRepository.RemoveFromCart(id, cartId);
        }
    }      
}
