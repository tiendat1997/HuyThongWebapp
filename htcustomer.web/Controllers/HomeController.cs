using htcustomer.web.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace htcustomer.web.Controllers
{
    [CustomAuthorize(Roles = "User")]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View("Home");
        }
    }
}