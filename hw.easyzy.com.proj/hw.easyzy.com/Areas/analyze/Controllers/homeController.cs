using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.analyze.Controllers
{
    public class homeController : Controller
    {
        public ActionResult Index(long zyId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);

            return View();
        }
    }
}