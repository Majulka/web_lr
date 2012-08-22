using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_LR.Models;
using System.Data;

namespace Web_LR.Controllers
{
    public class CultureController : Controller
    {
        private lrEntities db = new lrEntities();


        //
        // GET: /Culture/

        public ViewResult Index()
        {
            return View(db.Culture.ToList());
        }

        //
        // GET: /Culture/Details/5

        public ViewResult Details(int id)
        {
            Culture culture = db.Culture.Single(c => c.Id == id);
            return View(culture);
        }

        //
        // GET: /Culture/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Culture/Create

        [HttpPost]
        public ActionResult Create(Culture culture)
        {
            if (ModelState.IsValid)
            {
                db.Culture.AddObject(culture);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }

            return View(culture);
        }

        //
        // GET: /Culture/Edit/5

        public ActionResult Edit(int id)
        {
            Culture culture = db.Culture.Single(c => c.Id == id);
            return View(culture);
        }

        //
        // POST: /Culture/Edit/5

        [HttpPost]
        public ActionResult Edit(Culture culture)
        {
            if (ModelState.IsValid)
            {
                db.Culture.Attach(culture);
                db.ObjectStateManager.ChangeObjectState(culture, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Logon", "Account");
            }
            return View(culture);
        }

        //
        // GET: /Culture/Delete/5

        public ActionResult Delete(int id)
        {
            Culture culture = db.Culture.Single(c => c.Id == id);
            return View(culture);
        }

        //
        // POST: /Culture/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Culture culture = db.Culture.Single(c => c.Id == id);
            db.Culture.DeleteObject(culture);
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
