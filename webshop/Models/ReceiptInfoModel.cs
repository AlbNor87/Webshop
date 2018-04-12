using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webshop.Models
{
    public class ReceiptInfoModel
    {
        public List<OrderRowsModel> OrderRows { get; set; }

        public OrderInfoModel OrderInfo { get; set; }
    }
}
