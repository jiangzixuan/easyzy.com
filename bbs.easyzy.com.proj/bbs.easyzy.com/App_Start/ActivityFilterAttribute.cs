using bbs.easyzy.bll;
using bbs.easyzy.common;
using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bbs.easyzy.com
{
    /// <summary>
    /// 活跃度计算过滤器
    /// </summary>
    public sealed class ActivityFilterAttribute : ActionFilterAttribute
    {
        
        /// <summary>
        /// 如果登录，且control和action在计算activity的集合内，则添加活跃度
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string DesUserModel = Util.GetCookie("easyzy.user", "useridentity");
            if (!string.IsNullOrEmpty(DesUserModel))
            {
                string DesKey = Util.GetAppSetting("DesKey");
                UserCookieHelper.UserCookieModel u = UserCookieHelper.DescryptUserCookie(DesUserModel, DesKey);
                int uId = u._id;

                var controllerName = (filterContext.RouteData.Values["controller"]).ToString().ToLower();
                var actionName = (filterContext.RouteData.Values["action"]).ToString().ToLower();
                string key = string.Concat(controllerName, "_", actionName);
                int value = 0;
                var aw = ActivityWeightConst.ActivityWeights.TryGetValue(key, out value);
                if(aw)
                {
                    DateTime FirstCycleDay = DateTime.Now.AddDays(1 - Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))).Date;
                    B_Topic.AddActivity(uId, key, value, FirstCycleDay);
                }
            }
            base.OnActionExecuting(filterContext);
            
        }
    }
    
}