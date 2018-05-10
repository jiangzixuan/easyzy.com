using easyzy.sdk;
using m.easyzy.bll;
using m.easyzy.common;
using m.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m.easyzy.com.Controllers
{
    public class BaseController : Controller
    {
        protected int UserId = 0;
        protected T_User User = null;

        public BaseController()
        {
            string u = Util.GetCookie(Const.CookieName_User, Const.CookieVluew_UserId);
            if (!string.IsNullOrEmpty(u))
            {
                UserId = int.Parse(u);
                User = B_UserRedis.GetUser(UserId);
            }
            ViewBag.UserInfo = User;
        }

        #region 因为路由规则问题未解决，暂将公共方法放这里
        public void Exit()
        {
            try
            {
                Util.ClearCookies(Const.CookieName_User);
            }
            catch
            { }
        }
        #endregion
    }
}