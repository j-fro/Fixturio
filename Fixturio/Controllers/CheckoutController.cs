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
        public ActionResult Index()
        {
            return View();
        }

        // POST: Checkout
        [HttpPost]
        public ActionResult Index(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;
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