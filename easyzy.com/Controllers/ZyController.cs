using easyzy.bll;
using easyzy.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace easyzy.com.Controllers
{
    public class ZyController : Controller
    {
        public ActionResult add()
        {
            var zy = b_Zy.GetZy(1000000);
            return View();
        }
    }
}