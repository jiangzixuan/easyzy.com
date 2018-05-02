using hw.easyzy.bll;
using hw.easyzy.common;
using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Controllers
{
    public class BaseController : Controller
    {
        protected int UserId = 0;
        protected T_User User = null;

        public BaseController()
        {
            string u = Util.GetCookie(EasyzyConst.CookieName_User, EasyzyConst.CookieVluew_UserId);
            if (!string.IsNullOrEmpty(u))
            {
                UserId = int.Parse(u);
                User = B_UserRedis.GetUser(UserId);
            }
            ViewBag.UserInfo = User;
        }
    }
}