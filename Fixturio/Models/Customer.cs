using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fixturio.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(5)]
        public string Zip { get; set; }
        public virtual ICollection<UserName> Users { get; set; }
    }
}