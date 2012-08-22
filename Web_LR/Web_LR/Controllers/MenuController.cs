using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_LR.Models;
using System.Data;
using System.Threading;

namespace Web_LR.Controllers
{
    public class MenuController : Controller
    {
        private lrEntities db = new lrEntities();

        [ChildActionOnly]
        public PartialViewResult Render()
        {
            //var menu = db.Menu.Include("PageRoute");            
            var actualCulture = Thread.CurrentThread.CurrentCulture.ToString();
            var model = (from m in db.Menu where m.Culture1.Culture1 == actualCulture orderby m.ItemOrder select m).ToList();
            return PartialView("_Render", model);
        }

        [Authorize]
        public ViewResult Index()
        {
            //var menu = db.Menu.Include("PageRoute");
            //var menu = db.Menu.Include("PageRoute").Where(m => m.Culture == ActualCulture).OrderBy(m => m.ItemOrder);
            var actualCulture = Thread.CurrentThread.CurrentCulture.ToString();
            var menu = (from m in db.Menu where m.Culture1.Culture1 == actualCulture orderby m.ItemOrder select m).ToList();
            return View("Index", menu);
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Culture = new SelectList(db.Culture, "Id", "Culture1");
            ViewBag.Page = new SelectList(db.PageRoute, "Id", "PageAddress");
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menu.AddObject(menu);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }

            ViewBag.Culture = new SelectList(db.Culture, "Id", "Culture1", menu.Culture);
            ViewBag.Page = new SelectList(db.PageRoute, "Id", "PageAddress", menu.Page);
            return View(menu);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Menu menu = db.Menu.Single(m => m.Id == id);
            ViewBag.Page = new SelectList(db.PageRoute, "Id", "PageAddress", menu.Page);
            ViewBag.Culture = new SelectList(db.Culture, "Id", "Culture1", menu.Culture);
            return View(menu);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menu.Attach(menu);
                db.ObjectStateManager.ChangeObjectState(menu, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }

            ViewBag.Culture = new SelectList(db.Culture, "Id", "Culture1", menu.Culture);
            ViewBag.Page = new SelectList(db.PageRoute, "Id", "PageAddress", menu.Page);
            return View(menu);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Menu menu = db.Menu.Single(m => m.Id == id);
            return View(menu);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menu.Single(m => m.Id == id);
            db.Menu.DeleteObject(menu);
            db.SaveChanges();
            return RedirectToAction("Logon", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
