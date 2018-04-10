using System;
using System.Text;
using System.Collections.Generic;
using webshop.Models;


namespace webshop.Repositories.Implementations
{
    public interface ICartRepository
    {
        List<CartItemModel> Get(string cartId);

        bool AddToCart(int id, string cartId, int quantity);

        void RemoveFromCart(int id, string cartId);
    }
}

