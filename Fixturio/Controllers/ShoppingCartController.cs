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

        // AJAX: /ShoppingCart/AddToCart/5
        [HttpPost]
        public ActionResult AddToCartAJAX(int id)
        {
            var shoppingCart = ShoppingCart.GetCart(this.HttpContext);

            var element = storeDB.DisplayElements.Single(d => d.DisplayElementID == id);

            //var cart = storeDB.Carts.Single(c => c.DisplayElementID == id
            //                                && c.CartID == shoppingCart.ShoppingCartID);

            int itemCount = shoppingCart.AddToCart(element);

            var results = new ShoppingCartAddViewModel
            {
                Message = Server.HtmlEncode(element.Name) + " has been added to your cart",
                CartTotal = shoppingCart.GetCount(),
                CartCount = shoppingCart.GetCount(),
                ItemCount = itemCount,
                AddID = id
            };

            return Json(results);
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

        // AJAX: /ShoppingCart/RemoveFromCartFromBrowse/5
        [HttpPost]
        public ActionResult RemoveFromCartFromBrowse(int id)
        {
            var shoppingCart = ShoppingCart.GetCart(this.HttpContext);

            string elementName = storeDB.DisplayElements.Single(d => d.DisplayElementID == id).Name;

            var cart = storeDB.Carts.Single(c => c.DisplayElementID == id
                                            && c.CartID == shoppingCart.ShoppingCartID);

            int itemCount = shoppingCart.RemoveFromCart(cart.RecordID);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(elementName) + " has been removed from your cart",
                CartTotal = shoppingCart.GetCount(),
                CartCount = shoppingCart.GetCount(),
                ItemCount = itemCount,
                DeleteID = id
            };

            return Json(results);
        }

        // GET: /ShoppingCart/ItemCount/5
        [ChildActionOnly]
        public ActionResult ItemCount(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            
            var results = storeDB.Carts.SingleOrDefault(c => c.DisplayElementID == id
                                                        && c.CartID == cart.ShoppingCartID);
            if (results != null)
            {
                ViewData["Count"] = results.Count;
            }
            else
            {
                ViewData["Count"] = 0;
            }
            ViewData["ID"] = id;
            return PartialView("ItemCount");
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