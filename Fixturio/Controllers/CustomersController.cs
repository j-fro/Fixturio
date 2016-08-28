using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fixturio.Models;

namespace Fixturio.Controllers
{
    public class CustomersController : Controller
    {
        private DisplayElementDBContext db = new DisplayElementDBContext();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,Name,Address1,Address2,City,State,Zip")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var user = db.UserNames.SingleOrDefault(u => u.Name == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    user = new UserName
                    {
                        Name = HttpContext.User.Identity.Name
                    };
                    user.Customers = new List<Customer>();
                    user.Customers.Add(customer);
                    db.UserNames.Add(user);
                }

                customer.Users = new List<UserName>();
                customer.Users.Add(user);

                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,Name,Address1,Address2,City,State,Zip")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var user = db.UserNames.SingleOrDefault(u => u.Name == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    user = new UserName
                    {
                        Name = HttpContext.User.Identity.Name
                    };
                    user.Customers = new List<Customer>();
                    user.Customers.Add(customer);
                    db.UserNames.Add(user);
                }

                customer.Users = new List<UserName>();
                customer.Users.Add(user);

                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Customers/CustomerList
        [ChildActionOnly]
        public ActionResult CustomerList()
        {
            var user = (from u in db.UserNames
                        where u.Name == HttpContext.User.Identity.Name
                        select u).SingleOrDefault();

            var results = from cust in db.Customers
                          where cust.Users.Contains(user)
                          select cust; //.Name;
            // ViewData["Customers"] = results;
            return PartialView(results);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
