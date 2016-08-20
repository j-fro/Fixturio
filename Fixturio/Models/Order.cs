using System;
using System.Collections.Generic;

namespace Fixturio.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string Username { get; set; }
        public int Total { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}