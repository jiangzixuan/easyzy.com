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
                Ip = ClientUtil.Ip,
                IMEI = ClientUtil.IMEI,
                MobileBrand = ClientUtil.MobileBrand,
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

        public ActionResult Query(string ZyNum = "")
        {
            ViewBag.ZyNum = ZyNum;
            ViewBag.UploadUrl = Util.GetAppSetting("UploadUrlPrefix");
            return View();
        }

        public ActionResult QuerySubmitedStudents(string zyNum)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            List<T_Answer> al = B_Zy.GetZyAnswers(zyId);
            return PartialView(al);
        }

        /// <summary>
        /// 获取学生答题卡
        /// </summary>
        /// <param name="zyNum"></param>
        /// <param name="studentId"></param>
        /// <param name="trueName"></param>
        /// <returns></returns>
        public ActionResult GetStudentAnswerCard(string zyNum, string trueName)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            List<dto_Answer2> result = null;
            T_Answer a = B_ZyRedis.GetZyAnswer(zyId, trueName);   //获取学生提交的答案
            List<T_ZyStruct> zysl = B_ZyRedis.GetZyStruct(zyId);  //作业结构中可以获取正确答案
            if (a != null)
            {
                List<dto_Answer> ansl = JsonConvert.DeserializeObject<List<dto_Answer>>(a.AnswerJson);
                result = new List<dto_Answer2>();
                ansl.ForEach(s =>
                {
                    result.Add(new dto_Answer2() { StructId = s.StructId, BqNum = s.BqNum, SqNum = s.SqNum, Answer = s.Answer, QuesAnswer = zysl.FirstOrDefault(t => t.Id == s.StructId).QuesAnswer });
                });
                ViewBag.AnswerImg = a.AnswerImg;
            }
            ViewBag.TrueName = trueName;
            ViewBag.UploadUrl = Util.GetAppSetting("UploadUrlPrefix");
            return PartialView(result);
        }

        public ActionResult Check(string zyNum, string trueName)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            T_Zy zy = B_ZyRedis.GetZy(zyId);

            List<dto_Answer2> result = null;
            T_Answer a = B_ZyRedis.GetZyAnswer(zyId, trueName);   //获取学生提交的答案
            List<T_ZyStruct> zysl = B_ZyRedis.GetZyStruct(zyId);  //作业结构中可以获取正确答案
            if (a != null)
            {
                List<dto_Answer> ansl = JsonConvert.DeserializeObject<List<dto_Answer>>(a.AnswerJson);
                result = new List<dto_Answer2>();
                ansl.ForEach(s =>
                {
                    result.Add(new dto_Answer2() { StructId = s.StructId, BqNum = s.BqNum, SqNum = s.SqNum, Answer = s.Answer, QuesAnswer = zysl.FirstOrDefault(t => t.Id == s.StructId).QuesAnswer });
                });
                ViewBag.AnswerImg = a.AnswerImg;
                ViewBag.AnswerCard = ansl;
            }
            ViewBag.ZyNum = zyNum;
            ViewBag.TrueName = trueName;
            ViewBag.UploadUrl = Util.GetAppSetting("UploadUrlPrefix");
            ViewBag.BodyHtml = zy.BodyHtmlPath;
            ViewBag.AnswerHtml = zy.AnswerHtmlPath;
            return View();
        }
    }
}