using System;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilterAttribute());
        }
    }
}
