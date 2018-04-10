using System;
using System.Text;
using System.Collections.Generic;
using webshop.Models;


namespace webshop.Repositories.Implementations
{
    public interface ICheckoutRepository
    {
        List<CartItemModel> Get(string cartId);

        bool PlaceOrder(string firstname, string lastname, string email, string adress, int zipcode, string payment, string cartId, int sum);

        void RemoveFromCart(int id, string cartId);
    }
}

