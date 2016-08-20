using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fixturio.Models;
using log4net;

namespace Fixturio.Controllers
{
    public class DisplayElementsController : Controller
    {
        private DisplayElementDBContext db = new DisplayElementDBContext();
        readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: DisplayElements
        public ActionResult Index()
        {
            return View(db.DisplayElements.ToList());
        }

        // GET: DisplayElements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayElement displayElement = db.DisplayElements.Include(d => d.FilePaths).SingleOrDefault(d => d.DisplayElementID == id);
            if (displayElement == null)
            {
                return HttpNotFound();
            }
            return View(displayElement);
        }

        // GET: DisplayElements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DisplayElements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DisplayElementID,ModelNumber,Name,Length,Width,Height")] DisplayElement displayElement, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var photo = new FilePath
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.Photo
                    };
                    upload.SaveAs(System.IO.Path.Combine(Server.MapPath(@"~\images"), photo.FileName));
                    displayElement.FilePaths = new List<FilePath>();
                    displayElement.FilePaths.Add(photo);
                }
                db.DisplayElements.Add(displayElement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(displayElement);
        }

        // GET: DisplayElements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayElement displayElement = db.DisplayElements.Include(d => d.FilePaths).SingleOrDefault(d => d.DisplayElementID == id);
            if (displayElement == null)
            {
                return HttpNotFound();
            }
            return View(displayElement);
        }

        // POST: DisplayElements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DisplayElementID,ModelNumber,Name,Length,Width,Height")] DisplayElement displayElement, HttpPostedFileBase upload)
        {
            logger.Error("Entering Edit method");
            logger.Error(string.Format("Model state is {0}", ModelState.IsValid));
            logger.Error(string.Format("Upload is {0}, length {1}", upload, upload.ContentLength));

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    logger.Error("Upload is good");
                    var photo = new FilePath
                    {
                        DisplayElementId = displayElement.DisplayElementID,
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.Photo
                    };
                    upload.SaveAs(System.IO.Path.Combine(Server.MapPath(@"~\images"), photo.FileName));
                    logger.Error(string.Format("Saved as {0}", photo.FileName));
                    if (displayElement.FilePaths == null)
                    {
                        displayElement.FilePaths = new List<FilePath>();
                        logger.Error(string.Format("Creating a new filePath list for {0}", displayElement.DisplayElementID));
                    }
                    displayElement.FilePaths.Add(photo);
                    foreach (FilePath fp in displayElement.FilePaths)
                    {
                        logger.Error(string.Format("display has a filepath: {0}", fp.FilePathId));
                    }
                    db.FilePaths.Add(photo);
                }
                db.Entry(displayElement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(displayElement);
        }

        // GET: DisplayElements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayElement displayElement = db.DisplayElements.Find(id);
            if (displayElement == null)
            {
                return HttpNotFound();
            }
            return View(displayElement);
        }

        // POST: DisplayElements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DisplayElement displayElement = db.DisplayElements.Find(id);
            db.DisplayElements.Remove(displayElement);
            db.SaveChanges();
            return RedirectToAction("Index");
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
