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

        public ActionResult Login()
        {
            return View();
        }

        public string UserLogin(string userName, string passWord)
        {
            T_User u = B_UserRedis.GetUser(userName);
            if (u == null) return "1";
            if (u.Psd == Util.MD5(passWord))
            {
                DateTime dt = DateTime.Now.AddDays(30);
                
                Util.SetCookie(EasyzyConst.CookieName_User, EasyzyConst.CookieVluew_UserId, u.Id.ToString(), dt);
                return "0";
            }
            else
            {
                return "1";
            }
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
            string hasPsd = "";
            T_Zy zy = B_ZyRedis.GetZy(zyId);
            if (zy != null)
            {
                if (zy.UserId != 0)
                {
                    T_User u = B_UserRedis.GetUser(zy.UserId);
                    if (u.ZyPsd != "")
                    {
                        hasPsd = "1|";
                    }
                    else
                    {
                        hasPsd = "0|";
                    }
                }
                else
                {
                    hasPsd = "0|";
                }
            }

            return hasPsd + JsonConvert.SerializeObject(zy);
        }

        /// <summary>
        /// 判断作业是否有密码
        /// </summary>
        /// <param name="zyNum"></param>
        /// <returns></returns>
        public string HasZyPsd(string zyNum)
        {
            string result = "0";
            int zyId = EasyzyConst.GetZyId(zyNum);
            T_Zy zy = B_ZyRedis.GetZy(zyId);
            if (zy != null)
            {
                if (zy.UserId != 0)
                {
                    T_User u = B_UserRedis.GetUser(zy.UserId);
                    if (u.ZyPsd != "")
                    {
                        result = "1";
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 验证作业密码，返回作业信息
        /// </summary>
        /// <param name="zyNum"></param>
        /// <param name="zyPsd"></param>
        /// <returns></returns>
        public string CheckZyPsd(string zyNum, string zyPsd)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            string result = "1";
            T_Zy zy = B_ZyRedis.GetZy(zyId);
            if (zy != null)
            {
                if (zy.UserId != 0)
                {
                    T_User u = B_UserRedis.GetUser(zy.UserId);
                    if (u.ZyPsd == zyPsd)
                    {
                        result = "0";
                    }
                }
            }
            if (result == "0")
            {
                return result + "|" + JsonConvert.SerializeObject(zy);
            }
            else
            {
                return result + "|";
            }
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

        public ActionResult Down()
        {
            if (UserId == 0)
            {
                return View("login");
            }
            List<dto_UserZy> list = B_UserZy.GetSubmitedZy(UserId);
            if (list != null)
            {
                list.ForEach(a => a.ZyNum = EasyzyConst.GetZyNum(a.ZyId));
            }
            return View(list);
        }

        public ActionResult Created()
        {
            if (UserId == 0)
            {
                return View("login");
            }
            List<dto_UserZy> list = B_UserZy.GetUserZy(UserId);
            if (list != null)
            {
                list.ForEach(a => a.ZyNum = EasyzyConst.GetZyNum(a.ZyId));
            }
            return View(list);
        }
    }
}