﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fixturio.ViewModels
{
    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public int CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteID { get; set; }
    }
}