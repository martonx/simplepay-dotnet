using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class OrderItem
    {
        public string Ref { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        public int Tax { get; set; }
    }
}
