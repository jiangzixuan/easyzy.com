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
        public T_User User = null;
        public HomeController()
        {
            string u = Util.GetCookie(EasyzyConst.CookieName_User, EasyzyConst.CookieVluew_UserId);
            if (!string.IsNullOrEmpty(u))
            {
                User = B_UserRedis.GetUser(int.Parse(u));
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        
    }
}