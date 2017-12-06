using easyzy.bll;
using easyzy.common;
using easyzy.model.dto;
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

        /// <summary>
        /// 提交答案
        /// </summary>
        /// <param name="zyNum"></param>
        /// <param name="trueName"></param>
        /// <param name="objecttiveAnswer"></param>
        /// <param name="imgAnswer"></param>
        /// <returns></returns>
        public string SubmitAnswer(string zyNum, string trueName, string objecttiveAnswer, string imgAnswer)
        {
            if (string.IsNullOrEmpty(trueName))
            {
                return "1|真实姓名未填写！";
            }

            int zyId = EasyzyConst.GetZyId(zyNum);

            if (B_Zy.IsZySubmited(zyId, trueName))
            {
                return "1|此姓名已提交过作业！";
            }

            List<dto_Answer> al = new List<dto_Answer>();
            dto_Answer a = null;
            if (!string.IsNullOrEmpty(objecttiveAnswer))
            {
                List<T_ZyStruct> zysl = B_ZyRedis.GetZyStruct(zyId);
                string[] ol = objecttiveAnswer.Split('|');
                foreach (var o in ol)
                {
                    string[] oc = o.Split(',');
                    a = new dto_Answer();
                    a.StructId = int.Parse(oc[0]);
                    T_ZyStruct zys = zysl.First(s => s.Id == a.StructId);
                    a.BqNum = zys.BqNum;
                    a.SqNum = zys.SqNum;
                    a.Answer = oc[1];
                    al.Add(a);
                }
            }

            T_Answer t = new T_Answer()
            {
                ZyId = zyId,
                StudentId = UserId,
                TrueName = trueName,
                AnswerJson = JsonConvert.SerializeObject(al),
                AnswerImg = imgAnswer,
                CreateDate = DateTime.Now,
                Ip = ClientUtil.Ip,
                IMEI = ClientUtil.IMEI,
                MobileBrand = ClientUtil.MobileBrand,
                SystemType = Request.Browser.Platform.ToString(),
                Browser = Request.Browser.Browser.ToString()
            };

            int i = B_Zy.AddZyAnswer(t);

            return i > 0 ? ("0|" + B_ZyRedis.GetZy(zyId).AnswerHtmlPath) : "1|提交入库失败！";
        }
    }
}