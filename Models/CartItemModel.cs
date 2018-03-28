using System;
using System.ComponentModel.DataAnnotations;

namespace webshop.Models
{
    public class CartItemModel
    {
        [Key]
        public string CartId { get; set; }

        public int Quantity { get; set; }

        public string ProductId { get; set; }

        public int Price { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Image { get; set; }

    }
}
