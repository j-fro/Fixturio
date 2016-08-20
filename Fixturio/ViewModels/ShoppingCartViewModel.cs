using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fixturio.Models;

namespace Fixturio.ViewModels
{
    public class ShoppingCartViewModel
    {
        public int ID { get; set; }
        public List<Cart> CartItems { get; set; }
        public int CartTotal { get; set; }
    }
}