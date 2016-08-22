using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fixturio.Models
{
    public class Order
    {
        [ScaffoldColumn(false)]
        public int OrderID { get; set; }
        [ScaffoldColumn(false)]
        public string Username { get; set; }
        public int Total { get; set; }
        public virtual Customer Customer { get; set; }
        [ScaffoldColumn(false)]
        public DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}