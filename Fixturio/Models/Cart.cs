using System;
using System.ComponentModel.DataAnnotations;

namespace Fixturio.Models
{
    public class Cart
    {
        [Key]
        public int RecordID { get; set; }
        public string CartID { get; set; }
        public int DisplayElementID { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual DisplayElement DisplayElement { get; set; }
    }
}