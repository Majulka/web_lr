using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_LR.Models;
using System.Data;

namespace Web_LR.Controllers
{
    public class PageContentController : Controller
    {
        private lrEntities db = new lrEntities();

        [Authorize]
        public ViewResult Index(int id)
        {
            //var pagecontents = db.PageContents.Include("PageContentClasses").Include("PageRoute");
            //var pagecontents = from p in db.PageContents where p.PageId == id select p;
            var pagecontents = (from p in db.PageContents where p.PageId == id group p by p.CssClass into g select g.OrderByDescending(t => t.ChangeTime).FirstOrDefault()).ToList();
            ViewBag.PageId = id;
            return View(pagecontents);
        }

        [Authorize]
        public ViewResult History(int id)
        {
            //var pagecontents = db.PageContents.Include("PageContentClasses").Include("PageRoute");
            var pagecontents = from p in db.PageContents where p.PageId == id select p;
            ViewBag.PageId = id;
            return View(pagecontents.ToList());
        }

        [Authorize]
        public ActionResult HistoryBack(int id)
        {
            PageContents pagecontents = db.PageContents.Single(p => p.Id == id);
            pagecontents.ChangeTime = DateTime.Now;

            //db.PageContents.Attach(pagecontents);
            db.ObjectStateManager.ChangeObjectState(pagecontents, EntityState.Modified);
            db.SaveChanges();

            return RedirectToAction("Logon", "Account");
        }


        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.CssClass = new SelectList(db.PageContentClasses, "Id", "ClassName");
            ViewBag.PageId = id;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(PageContents pagecontents, int pageId)
        {
            pagecontents.PageId = pageId;
            pagecontents.ChangeTime = DateTime.Now;
            pagecontents.PageType = 1;
            if (ModelState.IsValid)
            {
                db.PageContents.AddObject(pagecontents);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }

            ViewBag.CssClass = new SelectList(db.PageContentClasses, "Id", "ClassName", pagecontents.CssClass);
            ViewBag.PageId = pageId;
            return View(pagecontents);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            PageContents pagecontents = db.PageContents.Single(p => p.Id == id);
            ViewBag.CssClass = new SelectList(db.PageContentClasses, "Id", "ClassName", pagecontents.CssClass);
            pagecontents.PageContent = HttpUtility.HtmlDecode(pagecontents.PageContent);
            //ViewBag.PageId = new SelectList(db.PageRoute, "Id", "PageAddress", pagecontents.PageId);
            return View(pagecontents);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(PageContents pagecontents)
        {
            if (ModelState.IsValid)
            {
                pagecontents.ChangeTime = DateTime.Now;
                db.PageContents.AddObject(pagecontents);
                db.ObjectStateManager.ChangeObjectState(pagecontents, EntityState.Added);
                //db.PageContents.Attach(pagecontents);
                //db.ObjectStateManager.ChangeObjectState(pagecontents, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }
            ViewBag.CssClass = new SelectList(db.PageContentClasses, "Id", "ClassName", pagecontents.CssClass);
            //ViewBag.PageId = new SelectList(db.PageRoute, "Id", "PageAddress", pagecontents.PageId);
            return View(pagecontents);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            PageContents pagecontents = db.PageContents.Single(p => p.Id == id);
            return View(pagecontents);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            PageContents pagecontents = db.PageContents.Single(p => p.Id == id);
            db.PageContents.DeleteObject(pagecontents);
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
