using easyzy.sdk;
using paper.easyzy.bll;
using paper.easyzy.model.entity;
using System.Web.Mvc;

namespace paper.easyzy.com.Controllers
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
                B_User User = new B_User(UserId);
                UserInfo = User.GetUser();
            }
            ViewBag.UserInfo = UserInfo;
        }
    }
}