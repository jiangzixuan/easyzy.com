using user.easyzy.bll;
using user.easyzy.model.entity;
using System;
using System.Web.Mvc;
using easyzy.sdk;

namespace user.easyzy.com.Controllers
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
        public string SaveSuggest(string content, string name, string phone)
        {
            T_Suggestion t = new T_Suggestion() { Name = name, Phone = phone, Content = content, CreateDate = DateTime.Now, Processed = false };
            int i = B_Common.AddSuggestion(t);
            return i > 0 ? "" : "Error";
        }

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