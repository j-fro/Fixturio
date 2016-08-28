using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fixturio.Models
{
    public class UserName
    {
        public int UserNameID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}