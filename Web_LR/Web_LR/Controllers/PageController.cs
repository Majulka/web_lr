using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_LR.Models;
using System.Data;

namespace Web_LR.Controllers
{
    public class PageController : BaseController
    {
        private lrEntities db = new lrEntities();


        public ViewResult Render()
        {
            string pageId = Request.RequestContext.HttpContext.Items["PageId"].ToString();
            int id;

            if (int.TryParse(pageId, out id))
            {
                if (id == 0)
                    id = 1;

                var model = (from p in db.PageContents where p.PageId == id group p by p.CssClass into g select g.OrderByDescending(t => t.ChangeTime).FirstOrDefault()).OrderByDescending(o => o.CssClass).ToList();

                ViewBag.Title = (from t in db.PageRoute where t.Id == id select t.PageName).FirstOrDefault();

                return View(model);

            }

            throw new ApplicationException();

        }

        [Authorize]
        public ViewResult Index()
        {
            var pageroute = db.PageRoute.Include("Culture1");
            return View(pageroute.ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Culture = new SelectList(db.Culture, "Id", "Culture1");
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(PageRoute pageroute)
        {
            if (ModelState.IsValid)
            {
                db.PageRoute.AddObject(pageroute);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }

            ViewBag.Culture = new SelectList(db.Culture, "Id", "Culture1", pageroute.Culture);
            return View(pageroute);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            PageRoute pageroute = db.PageRoute.Single(p => p.Id == id);
            ViewBag.Culture = new SelectList(db.Culture, "Id", "Culture1", pageroute.Culture);
            return View(pageroute);
        }

        public ActionResult EditHtml()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(PageRoute pageroute)
        {
            if (ModelState.IsValid)
            {
                db.PageRoute.Attach(pageroute);
                db.ObjectStateManager.ChangeObjectState(pageroute, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }
            ViewBag.Culture = new SelectList(db.Culture, "Id", "Culture1", pageroute.Culture);
            return View(pageroute);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            PageRoute pageroute = db.PageRoute.Single(p => p.Id == id);
            return View(pageroute);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            PageRoute pageroute = db.PageRoute.Single(p => p.Id == id);
            db.PageRoute.DeleteObject(pageroute);
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
