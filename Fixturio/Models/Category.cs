using System.ComponentModel.DataAnnotations;

namespace Fixturio.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}