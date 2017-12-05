using easyzy.bll;
using easyzy.common;
using easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m.easyzy.com.Controllers
{
    /// <summary>
    /// 公共功能
    /// </summary>
    public class CommonController : Controller
    {
        protected int UserId = 0;
        protected T_User User = null;

        public CommonController()
        {
            string u = Util.GetCookie(EasyzyConst.CookieName_User, EasyzyConst.CookieVluew_UserId);
            if (!string.IsNullOrEmpty(u))
            {
                UserId = int.Parse(u);
                User = B_UserRedis.GetUser(UserId);
            }
            ViewBag.UserInfo = User;
        }

        public void Exit()
        {
            try
            {
                Util.ClearCookies(EasyzyConst.CookieName_User);
            }
            catch
            { }
        }
    }
}