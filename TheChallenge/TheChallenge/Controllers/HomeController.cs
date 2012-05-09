using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheChallenge.Helpers;

namespace TheChallenge.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.isMobileOn = this.Request.Browser.IsMobileDevice;
            ViewBag.settings = this.Request.Browser;
            return View();
        }
    }
}
