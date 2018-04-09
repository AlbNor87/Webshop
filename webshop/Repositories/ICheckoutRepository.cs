using System;
using System.Text;
using System.Collections.Generic;
using webshop.Models;


namespace webshop.Repositories.Implementations
{
    public interface ICheckoutRepository
    {
        List<CartItemModel> Get(string cartId);

        void AddToCart(int id, string cartId, int quantity);

        void RemoveFromCart(int id, string cartId);
    }
}

