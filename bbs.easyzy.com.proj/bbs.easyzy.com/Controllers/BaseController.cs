using bbs.easyzy.model.entity;
using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bbs.easyzy.com.Controllers
{
    public class BaseController : Controller
    {
        protected int UserId = 0;
        protected T_User User = null;

        public BaseController()
        {
            //2个问题，1，cookie加密、访问；2，调用用户服务数据
            string u = Util.GetCookie(Const.CookieName_User, Const.CookieVluew_UserId);
            if (!string.IsNullOrEmpty(u))
            {
                UserId = int.Parse(u);
                //User = B_UserRedis.GetUser(UserId);
            }
            ViewBag.UserInfo = User;
        }
    }
}