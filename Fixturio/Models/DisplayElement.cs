using System.Collections.Generic;
using System.Data.Entity;

namespace Fixturio.Models
{
    public class DisplayElement
    {
        public int DisplayElementID { get; set; }
        public string ModelNumber { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public virtual Category Category { get; set; }
        public virtual ICollection<FilePath> FilePaths { get; set; }
    }

    public class DisplayElementDBContext : DbContext
    {
        public DbSet<DisplayElement> DisplayElements { get; set; }
        public DbSet<FilePath> FilePaths { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<ViewModels.ShoppingCartViewModel> ShoppingCartViewModels { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserName> UserNames { get; set; }
    }
}