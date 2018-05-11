using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.App_Start
{
    /// <summary>
    /// 登录验证过滤器
    /// </summary>
    public sealed class LoginFilter : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (WebHelper.IsAjax())
            //{
            //    //也要判断是否登录
            //    LoginPcTokenID loginPcToken = Util.GetCurrentIdentity();
            //    if (loginPcToken != null && loginPcToken.IsLegal)
            //    {
            //        base.OnActionExecuting(filterContext);
            //    }
            //    else
            //    {
            //        JsonResult json = new JsonResult();
            //        json.Data = new { status = "100", message = "非法请求" };
            //        json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //        filterContext.Result = json;
            //    }
            //}
            //else
            //{
            //    LoginPcTokenID loginPcToken = Util.GetUserIdentity();
            //    if (loginPcToken == null)
            //    {
            //        filterContext.Result = new RedirectResult("/Home/Login");
            //    }
            //    else
            //    {
            //        //判断学校是否过期
            //        M_UserInfo u = B_User.GetUserInfo(loginPcToken.Uid);
            //        if (u.UserType == 2 && !ZxxkUserRD.IsSchoolValid(u.SchoolID))
            //        {
            //            filterContext.Result = new RedirectResult("/Home/Login");
            //        }
            //        else
            //        {
            //            bool isajax = filterContext.HttpContext.Request.Headers["X-Requested-With"] == null ? false : true;
            //            if (!isajax)
            //            {
            //                int UserType = u.UserType;
            //                string RedirectPath = "";
            //                switch (UserType)
            //                {
            //                    case 1: RedirectPath = "/Student/HomeWork/HomeWorkList"; break;
            //                    case 2: RedirectPath = "/Teacher/HomeWork/List"; break;
            //                    case 3: RedirectPath = "/SysAdmin/Sys/Index"; break;
            //                }

            //                if (filterContext.Controller.ControllerContext.RouteData.DataTokens["area"] != null)
            //                {
            //                    string Area = filterContext.Controller.ControllerContext.RouteData.DataTokens["area"].ToString();
            //                    if (Area == "Student" && UserType != 1)
            //                    {
            //                        filterContext.Result = new RedirectResult(RedirectPath);
            //                    }
            //                    else if ((Area == "Teacher" || Area == "Admin" || Area == "HeadMaster" || Area == "SubjectDirector" || Area == "GradeDirector") && UserType != 2)
            //                    {
            //                        filterContext.Result = new RedirectResult(RedirectPath);
            //                    }
            //                    else if (Area == "SysAdmin" && UserType != 3)
            //                    {
            //                        filterContext.Result = new RedirectResult(RedirectPath);
            //                    }
            //                }
            //            }
            //            for (int i = 0; i < JQZYConst.TActivities.GetLength(0); i++)
            //            {
            //                if (JQZYConst.TActivities[i, 0].ToString() == HttpContext.Current.Request.Path.ToLower())
            //                {
            //                    B_User.AddTeacherActivity(loginPcToken.Uid, JQZYConst.TActivities[i, 0].ToString(), int.Parse(JQZYConst.TActivities[i, 2].ToString()));
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //    base.OnActionExecuting(filterContext);
            //}
        }
    }
    
}