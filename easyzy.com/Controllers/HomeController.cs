using easyzy.bll;
using easyzy.common;
using easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace easyzy.com.Controllers
{
    public class HomeController : CommonController
    {
        public ActionResult Index()
        {
            return View();
        }
        
    }
}