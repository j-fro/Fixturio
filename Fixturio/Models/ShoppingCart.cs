using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fixturio.Models
{
    public partial class ShoppingCart
    {
        DisplayElementDBContext storeDB = new DisplayElementDBContext();
        string ShoppingCartID { get; set; }
        public const string CartSessionKey = "CardID";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartID = cart.GetCartID(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public string GetCartID(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID
                    Guid tempCartID = Guid.NewGuid();
                    // Send temp ID back to client as a cookie
                    context.Session[CartSessionKey] = tempCartID.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void AddToCart(DisplayElement element)
        {
            // Get the matching cart and element instances
            var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartID == ShoppingCartID
                && c.DisplayElementID == element.DisplayElementID);

            if (cartItem == null)
            {
                // Create a new cart if one doesn't exist
                cartItem = new Cart
                {
                    DisplayElementID = element.DisplayElementID,
                    CartID = ShoppingCartID,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                storeDB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in a cart, incremenet the count
                cartItem.Count++;
            }
            storeDB.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            // Get the matching cart
            var cartItem = storeDB.Carts.Single(
                c => c.CartID == ShoppingCartID
                && c.RecordID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                storeDB.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where(
                c => c.CartID == ShoppingCartID);

            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            storeDB.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(
                c => c.CartID == ShoppingCartID).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and return the sum
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartID == ShoppingCartID
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public int CreateOrder(Order order)
        {
            int orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    DisplayElementID = item.DisplayElementID,
                    OrderID = order.OrderID,
                    Quantity = item.Count
                };
                // Set the total qty of the cart
                orderTotal += item.Count;

                storeDB.OrderDetails.Add(orderDetail);
            }
            order.Total = orderTotal;

            storeDB.SaveChanges();

            EmptyCart();

            return order.OrderID;
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.Carts.Where(
                c => c.CartID == ShoppingCartID);

            foreach (Cart item in shoppingCart)
            {
                item.CartID = userName;
            }
            storeDB.SaveChanges();
        }
    }
}