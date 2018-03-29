using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webshop.Models
{
    public class OrderInfoModel
    {
        public int Id { get; set; }

        public string FirstName  { get; set; }

        public string LastName { get; set; }

        public string Email{ get; set; }

        public string Adress { get; set; }

        public int ZipCode { get; set; }

        public string PaymentMethod { get; set; }

        public int Sum { get; set; }

        public string CartId { get; set; }

    }
}
