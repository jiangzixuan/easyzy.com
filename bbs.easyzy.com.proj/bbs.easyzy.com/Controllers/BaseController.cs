using bbs.easyzy.bll;
using bbs.easyzy.model.entity;
using easyzy.sdk;
using System.Web.Mvc;

namespace bbs.easyzy.com.Controllers
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
            int invites = 0;
            if (UserId != 0)
            {
                UserInfo = B_UserRedis.GetUser(UserId);
                invites = B_Topic.GetInviteCount(UserInfo.UserName);
            }
            
            ViewBag.UserInfo = UserInfo;
            ViewBag.Invites = invites;
        }
    }
}