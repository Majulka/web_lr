using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;

namespace Web_LR.Controllers
{
    [Omu.Awesome.Mvc.WhiteSpaceFilter]
    public class BaseController : Controller
    {
        public string ActualCulture
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.ToString();
            }
        }
    }
}
