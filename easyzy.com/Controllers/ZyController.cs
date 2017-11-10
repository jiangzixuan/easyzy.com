using easyzy.bll;
using easyzy.common;
using easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace easyzy.com.Controllers
{
    public class ZyController : Controller
    {
        public int UserId = 0;
        public ZyController()
        {
            string u = Util.GetCookie("easyzy", "UserID");
            if (!string.IsNullOrEmpty(u))
            {
                UserId = int.Parse(u);
            }
        }
        public ActionResult Add()
        {
            //var zy = b_Zy.GetZy(1000000);

            ViewBag.WordFunc = (int)EasyzyConst.WordFunc.CreateZY;
            return View();
        }

        public string CreateZy(string bodyWordPath, string bodyHtmlPath, string answerWordPath, string answerHtmlPath)
        {
            var context = System.Web.HttpContext.Current.Request.UserAgent;
            T_Zy zy = new T_Zy()
            {
                UserId = UserId,
                BodyWordPath = bodyWordPath,
                BodyHtmlPath = bodyHtmlPath,
                AnswerWordPath = answerWordPath,
                AnswerHtmlPath = answerHtmlPath,
                CreateDate = DateTime.Now,
                Ip = Util.GetUserIP(),
                IMEI = "",
                MobileBrand = "",
                SystemType = Request.Browser.Platform.ToString(),
                Browser = Request.Browser.Browser.ToString()
            };
            int Id = b_Zy.Create(zy);
            return Id > 0 ? EasyzyConst.GetZyNum(Id) : "";
        }
    }
}