using easyzy.bll;
using easyzy.common;
using easyzy.model.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m.easyzy.com.Controllers
{
    public class HomeController : CommonController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Open()
        {
            ViewBag.UploadUrl = Util.GetAppSetting("UploadUrlPrefix");
            ViewBag.PicFunc = (int)EasyzyConst.ImgFunc.SubmitAnswer;
            return View();
        }

        public string QueryZy(string zyNum)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            T_Zy zy = B_ZyRedis.GetZy(zyId);

            return JsonConvert.SerializeObject(zy);
        }

        public ActionResult GetZyStruct(string zyNum)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            T_Zy zy = B_ZyRedis.GetZy(zyId);
            List<T_ZyStruct> zys = null;
            if (zy.Structed)
            {
                zys = B_ZyRedis.GetZyStruct(zyId);
            }

            return PartialView(zys);
        }
    }
}