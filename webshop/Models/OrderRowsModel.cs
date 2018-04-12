using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webshop.Models
{
    public class OrderRowsModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Quantity { get; set; }

        public string Price { get; set; }
    }
}
