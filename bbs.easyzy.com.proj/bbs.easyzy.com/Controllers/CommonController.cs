using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bbs.easyzy.com.Controllers
{
    public class CommonController : Controller
    {
        public void Exit()
        {
            try
            {
                Util.ClearCookies("easyzy.user");
            }
            catch
            { }
        }
    }
}