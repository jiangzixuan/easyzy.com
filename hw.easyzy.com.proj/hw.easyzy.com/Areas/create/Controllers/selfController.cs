using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.create.Controllers
{
    public class selfController : baseController
    {
        public ActionResult Index()
        {
            ViewBag.WordFunc = (int)Const.WordFunc.CreateZY;
            ViewBag.UploadUrl = Util.GetAppSetting("UploadUrlPrefix");
            return View();
        }
        

        //public string CreateZy(string bodyWordPath, string bodyHtmlPath, string answerWordPath, string answerHtmlPath)
        //{
        //    var context = System.Web.HttpContext.Current.Request.UserAgent;
        //    T_Zy zy = new T_Zy()
        //    {
        //        UserId = UserId,
        //        BodyWordPath = bodyWordPath,
        //        BodyHtmlPath = bodyHtmlPath,
        //        AnswerWordPath = answerWordPath,
        //        AnswerHtmlPath = answerHtmlPath,
        //        CreateDate = DateTime.Now,
        //        Ip = ClientUtil.Ip,
        //        IMEI = ClientUtil.IMEI,
        //        MobileBrand = ClientUtil.MobileBrand,
        //        SystemType = Request.Browser.Platform.ToString(),
        //        Browser = Request.Browser.Browser.ToString(),
        //        Structed = false
        //    };
        //    int Id = B_Zy.Create(zy);
        //    return Id > 0 ? Const.GetZyNum(Id) : "";
        //}

        //public string CreateZyStruct(string zyNum, string structString)
        //{
        //    int zyId = Const.GetZyId(zyNum);
        //    string[] quesList = structString.Split('|');
        //    string[] quesCol;
        //    List<T_ZyStruct> zysl = new List<T_ZyStruct>();
        //    T_ZyStruct zys = null;
        //    foreach (var q in quesList)
        //    {
        //        quesCol = q.Split('-');
        //        zys = new T_ZyStruct() { ZyId = zyId, BqNum = int.Parse(quesCol[0]), SqNum = string.IsNullOrEmpty(quesCol[1]) ? 0 : int.Parse(quesCol[1]), QuesType = int.Parse(quesCol[2]), QuesAnswer = quesCol[3], CreateDate = DateTime.Now };
        //        zysl.Add(zys);
        //    }
        //    int i = B_Zy.AddZyStruct(zysl);
        //    if (B_Zy.UpdateZyStructed(zyId) > 0)
        //    {
        //        B_ZyRedis.UpdateZyStructed(zyId);
        //    }
        //    return i > 0 ? "" : "Error";
        //}
    }
}