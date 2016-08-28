using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using Fixturio.Models;

namespace Fixturio.Controllers
{
    
    public class BrowseController : Controller
    {
        private DisplayElementDBContext db = new DisplayElementDBContext();

        // GET: Browse
        [Authorize]
        public ActionResult Index()
        {
            return View(db.DisplayElements.Include(d => d.FilePaths).ToList());
        }

        // GET: /Browse/CategoryMenu
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = db.Categories.ToList();
            return PartialView(categories);
        }
    }
}