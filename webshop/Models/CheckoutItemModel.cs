using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace webshop.Models
{
    public class CheckoutItemModel
    {
        
        public List<CartItemModel> Cart { get; set; }

        public int Sum { get; set; }

        public string Message { get; set; }
       
    }
}
