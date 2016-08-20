using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fixturio.Models;
using Fixturio.ViewModels;

namespace Fixturio.Controllers
{
    public class ShoppingCartController : Controller
    {
        DisplayElementDBContext storeDB = new DisplayElementDBContext();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetCount()
            };

            return View(viewModel);
        }

        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            var addedElement = storeDB.DisplayElements
                .Single(e => e.DisplayElementID == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedElement);

            return RedirectToAction("Index");
        }

        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string elementName = storeDB.Carts.Single(c => c.RecordID == id).DisplayElement.Name;

            int itemCount = cart.RemoveFromCart(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(elementName) + " has been removed from your cart",
                CartTotal = cart.GetCount(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteID = id
            };

            return Json(results);
        }

        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}