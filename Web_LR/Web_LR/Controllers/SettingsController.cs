using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_LR.Models;

namespace Web_LR.Controllers
{
    public class SettingsController : Controller
    {
        [HttpGet]
        public ActionResult Language(string lang)
        {
            switch (lang)
            {
                case "cs": Response.Cookies["Culture"].Value = "cs-CZ"; break;
                case "en": Response.Cookies["Culture"].Value = "en-US"; break;
                case "de": Response.Cookies["Culture"].Value = "de-DE"; break;
            }

            return RedirectToAction("", "");
        }

    }
}
