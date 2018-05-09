using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bbs.easyzy.com.Controllers
{
    public class topicController : Controller
    {
        [HttpPost]
        public ActionResult add()
        {
            return View();
        }

        [HttpGet]
        public ActionResult list()
        {
            return View();
        }

        [HttpGet]
        public ActionResult detail(int id)
        {
            return View();
        }


    }
}