using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using System.Threading;

namespace Web_LR
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            //List<Web_LR.Models.PageRoute> pageRoute = (from p in new Web_LR.Models.Web_LREntities().PageRoute select p).ToList();

            //foreach (var item in pageRoute)
            //{
            //    routes.MapRoute(
            //        string.Format("{0}_Page", item.Id), // Route name
            //        item.PageAddress, // URL with parameters
            //        new { controller = "Page", action = "Render", id = UrlParameter.Optional } // Parameter defaults
            //    );
            //}

            routes.MapRoute(
                "Admin", // Route name
                "Admin", // URL with parameters
                new { controller = "Account", action = "LogOn" } // Parameter defaults
            );


            routes.MapRoute(
                "Teacher route main", // route name
                "", // url
                new { controller = "Page", action = "Render" } // defaults                
            );

            routes.MapRoute(
                "Teacher route", // route name
                "{teacherCode}", // url
                new { controller = "Page", action = "Render" } // defaults                
            );

            routes.MapRoute(
                "lan", // route name
                "language/{lang}", // url
                new { controller = "Settings", action = "Language", lang = UrlParameter.Optional } // defaults                
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Page", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Culture"];
            string actualCulture = "cs-CZ";
            if (cookie != null)
            {
                CultureInfo culture = new CultureInfo(cookie.Value);
                culture.NumberFormat.CurrencyDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                actualCulture = cookie.Value;
            }
            else
            {
                //CultureInfo culture = new CultureInfo("en-US");
                CultureInfo culture = new CultureInfo("cs-CZ");
                culture.NumberFormat.CurrencyDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            string requestLower = Request.Url.LocalPath.ToLower().TrimStart('/').TrimEnd('/');

            var pageId = (from p in new Web_LR.Models.lrEntities().PageRoute where p.PageAddress.ToLower() == requestLower && p.Culture1.Culture1 == actualCulture select p.Id).FirstOrDefault();

            //if (pageId != 0)
            Context.Items["PageId"] = pageId;
        }

    }
}