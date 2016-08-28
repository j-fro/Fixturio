using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fixturio.Models;

namespace Fixturio.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        DisplayElementDBContext storeDB = new DisplayElementDBContext();

        // GET: Checkout
        public ActionResult OrderDetails()
        {
            var user = (from u in storeDB.UserNames
                        where u.Name == HttpContext.User.Identity.Name
                        select u).SingleOrDefault();

            var customers = from c in storeDB.Customers
                            where c.Users.Any(u => u.Name == HttpContext.User.Identity.Name)
                            select c.Name;

            foreach (string cust in customers)
            {
                Console.WriteLine(cust);
            }
            ViewBag.Customers = new SelectList(customers);
            return View();
        }

        // POST: Checkout
        [HttpPost]
        public ActionResult OrderDetails(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;

                var customer = (from c in storeDB.Customers
                                where c.Name == values["Customer"].ToString()
                                select c).FirstOrDefault();

                order.Customer = customer;
                // Save order
                storeDB.Orders.Add(order);
                storeDB.SaveChanges();
                // Process order
                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.CreateOrder(order);

                return RedirectToAction("Complete", new { id = order.OrderID });
            }
            catch
            {
                // Invalid order - redisplay wtih errors
                return View(order);
            }
        }

        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderID == id
                && o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}