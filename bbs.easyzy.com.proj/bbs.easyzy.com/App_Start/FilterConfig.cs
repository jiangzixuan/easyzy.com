using System.Web.Mvc;

namespace bbs.easyzy.com
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilterAttribute());
            filters.Add(new ActivityFilterAttribute());
        }
        
    }
}
