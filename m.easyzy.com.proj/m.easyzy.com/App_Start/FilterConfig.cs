﻿using easyzy.sdk;
using System.Web;
using System.Web.Mvc;

namespace m.easyzy.com
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilterAttribute());
        }
        
    }
}
