namespace Fixturio.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int DisplayElementID { get; set; }
        public int Quantity { get; set; }
        public bool Filled { get; set; }
        public virtual DisplayElement DisplayElement { get; set; }
        public virtual Order Order { get; set; }
    }
}