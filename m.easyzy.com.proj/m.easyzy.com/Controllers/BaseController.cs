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
        protected T_User UserInfo = null;

        public BaseController()
        {
            string DesUserModel = Util.GetCookie("easyzy.user", "useridentity");
            string DesKey = Util.GetAppSetting("DesKey");
            UserCookieHelper.UserCookieModel u = UserCookieHelper.DescryptUserCookie(DesUserModel, DesKey);

            UserId = u._id;
            if (UserId != 0)
            {
                UserInfo = B_UserRedis.GetUser(UserId);
            }
            ViewBag.UserInfo = UserInfo;
        }

        #region 因为路由规则问题未解决，暂将公共方法放这里
        public void Exit()
        {
            try
            {
                Util.ClearCookies("easyzy.user");
            }
            catch
            { }
        }
        #endregion
    }
}