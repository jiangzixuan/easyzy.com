using easyzy.bll;
using easyzy.common;
using easyzy.model.entity;
using Newtonsoft.Json;
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
            ViewBag.UploadUrl = Util.GetAppSetting("UploadUrlPrefix");
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
                Browser = Request.Browser.Browser.ToString(),
                Structed = false
            };
            int Id = B_Zy.Create(zy);
            return Id > 0 ? EasyzyConst.GetZyNum(Id) : "";
        }

        public string CreateZyStruct(string zyNum, string structString)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            string[] quesList = structString.Split('|');
            string[] quesCol;
            List<T_ZyStruct> zysl = new List<T_ZyStruct>();
            T_ZyStruct zys = null;
            foreach (var q in quesList)
            {
                quesCol = q.Split('-');
                zys = new T_ZyStruct() { ZyId = zyId, BqNum = int.Parse(quesCol[0]), SqNum = string.IsNullOrEmpty(quesCol[1]) ? 0 : int.Parse(quesCol[1]), QuesType = int.Parse(quesCol[2]), QuesAnswer = quesCol[3], CreateDate = DateTime.Now };
                zysl.Add(zys);
            }
            int i = B_Zy.AddZyStruct(zysl);
            B_Zy.UpdateZyStructed(zyId);
            B_ZyRedis.DeleteZyCache(zyId);
            return i > 0 ? "" : "Error";
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
            if (zy != null)
            {
                zy.BodyHtmlPath = zy.BodyHtmlPath;
                zy.AnswerHtmlPath = zy.AnswerHtmlPath;
            }

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