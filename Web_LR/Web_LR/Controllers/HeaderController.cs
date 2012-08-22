using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_LR.Models;
using System.Threading;
using System.Data;

namespace Web_LR.Controllers
{
    public class HeaderController : Controller
    {
        private lrEntities db = new lrEntities();

        [ChildActionOnly]
        public PartialViewResult Render()
        {
            var actualCulture = Thread.CurrentThread.CurrentCulture.ToString();
            var model = (from m in db.Header where m.Culture1.Culture1 == actualCulture select m).ToList();
            return PartialView("_Render", model);
        }

        [Authorize]
        public ViewResult Index()
        {
            return View(db.Header.ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Header header)
        {
            if (ModelState.IsValid)
            {
                db.Header.AddObject(header);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }

            return View(header);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Header header = db.Header.Single(h => h.Id == id);
            ViewBag.Culture = new SelectList(db.Culture, "Id", "Culture1", header.Culture);
            ViewBag.Class = new SelectList(db.HeaderClasses, "Id", "ClassName", header.Class);
            return View(header);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Header header, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                header.IsImage = true;

                var fileName = System.IO.Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = System.IO.Path.Combine(Server.MapPath("~/Content/images"), fileName);
                file.SaveAs(path);

                header.ImagePath = fileName;
            }
            if (ModelState.IsValid)
            {
                db.Header.Attach(header);
                db.ObjectStateManager.ChangeObjectState(header, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }
            return View(header);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Header header = db.Header.Single(h => h.Id == id);
            return View(header);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Header header = db.Header.Single(h => h.Id == id);
            db.Header.DeleteObject(header);
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
