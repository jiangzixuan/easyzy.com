using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.analyze.Controllers
{
    public class baseController : Controller
    {
        protected int UserId = 0;
        protected T_User UserInfo = null;

        public baseController()
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
    }
}