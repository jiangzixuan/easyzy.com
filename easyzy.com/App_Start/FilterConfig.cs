using easyzy.common;
using System.Web;
using System.Web.Mvc;

namespace easyzy.com
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilterAttribute());

        }

        /// <summary>
        /// 统一日志处理
        /// </summary>
        public class ExceptionFilterAttribute : HandleErrorAttribute
        {
            public override void OnException(ExceptionContext filterContext)
            {
                var path = $"{filterContext.RouteData.GetRequiredString("controller")}/{filterContext.RouteData.GetRequiredString("action")}";
                var message = $"消息类型：{filterContext.Exception.GetType().Name}\r\n" +
                                $"消息内容：{filterContext.Exception.Message}\r\n" +
                                $"异常方法：{filterContext.Exception.TargetSite}\r\n" +
                                $"异常对象：{filterContext.Exception.Source}\r\n" +
                                $"异常目录：{path}\r\n" +
                                $"堆栈信息: {filterContext.Exception.StackTrace}";
                var para = (HttpRequestWrapper)((HttpContextWrapper)filterContext.HttpContext).Request;
                var context = HttpContext.Current.Request.UserAgent;
                //var userId = int.Parse(string.IsNullOrEmpty(Util.GetCookie("UserID")) ? "0" : Util.GetCookie("UserID"));
                var userParam = $"\r\n异常用户:{0}\r\n请求设备:{context}\r\n异常路由:{para.Url}";

                LogHelper.Error(message + userParam);
                base.OnException(filterContext);
            }
        }
    }
}
