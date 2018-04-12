using System;
using System.Collections.Generic;
using webshop.Models;
using webshop.Repositories.Implementations;
using webshop.Repositories;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;


namespace webshop.Services.Implementations
{
    public class CheckoutService
    {
        private readonly ICheckoutRepository checkoutRepository;

        public CheckoutService(ICheckoutRepository checkoutRepository)
        {
            this.checkoutRepository = checkoutRepository;
        }

        public CheckoutItemModel Get(string cartId)
        {

            var checkoutItems = this.checkoutRepository.Get(cartId);

            if(!checkoutItems.Any())
            {
                return null;
            }

            var sum = checkoutItems.Select(c => c.Price * c.Quantity).Sum();

            var checkoutItem = new CheckoutItemModel();
            checkoutItem.Cart = checkoutItems;
            checkoutItem.Sum = sum;

            return checkoutItem;
        }

        public (int OrderId, bool Success) PlaceOrder(string firstname, string lastname, string email, string adress, int zipcode, string payment, string cartId, int sum)
        {
            return this.checkoutRepository.PlaceOrder(firstname, lastname, email, adress, zipcode, payment, cartId, sum);
        }

        public ReceiptInfoModel GetOrderInfo(int orderId)
        {
            
            return this.checkoutRepository.GetOrderInfo(orderId);

        }
    }
}
