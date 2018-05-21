using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.list.Controllers
{
    public class homeController : Controller
    {
        // GET: list/home
        public ActionResult Index()
        {
            return View();
        }
    }
}