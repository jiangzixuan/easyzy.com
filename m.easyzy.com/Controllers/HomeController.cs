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
            string hasPsd = "0";
            T_Zy zy = B_ZyRedis.GetZy(zyId);
            //作业不存在
            if (zy == null) return "null";
            //判断打开权限
            string msg = "";
            if (!OpenAccess(zy, ref msg))
            {
                return "2|" + msg;
            }
            if (zy.UserId != 0)
            {
                T_User u = B_UserRedis.GetUser(zy.UserId);
                if (u.ZyPsd != "")
                {
                    hasPsd = "1|";
                }
            }
            //需要密码，则不返回作业html地址
            if (hasPsd == "0")
            {
                hasPsd = hasPsd + "|" + JsonConvert.SerializeObject(zy);
            }

            return hasPsd;
        }

        /// <summary>
        /// 判断打开权限
        /// 如果建作业的老师并未登录，那么认为是试用账号，其作业可以随便打开
        /// 如果建作业的老师是登录过的，那么要想打开作业，需满足条件：
        /// 1、已登录状态
        /// 2、没有提交过此作业
        /// </summary>
        /// <param name="zy"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool OpenAccess(T_Zy zy, ref string msg)
        {
            if (zy.UserId == 0) return true;
            if (UserId == 0)
            {
                msg = "您必须登录才能打开此作业！";
                return false;
            }
            if (B_ZyRedis.GetZyAnswer(zy.Id, UserId) != null)
            {
                msg = "您已提交过此作业，不能重复提交！";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断作业是否有密码
        /// </summary>
        /// <param name="zyNum"></param>
        /// <returns></returns>
        public string NeedZyPsd(string zyNum)
        {
            string result = "0";
            int zyId = EasyzyConst.GetZyId(zyNum);
            T_Zy zy = B_ZyRedis.GetZy(zyId);
            if (zy != null)
            {
                if (zy.UserId != 0 && zy.UserId != UserId)
                {
                    T_User u = B_UserRedis.GetUser(zy.UserId);
                    if (u.ZyPsd != "" && B_ZyRedis.GetZyAnswer(zyId, UserId) == null)
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
            List<dto_Answer3> dal = new List<dto_Answer3>();
            if (al != null && al.Count > 0)
            {
                List<T_ZyStruct> zysl = B_ZyRedis.GetZyStruct(zyId);
                int ObjectiveQuesCount = zysl.Count(a => a.QuesType == 0);
                int ObjectiveQuesCorrectCount = 0;

                foreach (var a in al)
                {
                    //计算客观题正确数量
                    List<dto_Answer> xx = JsonConvert.DeserializeObject<List<dto_Answer>>(a.AnswerJson);
                    ObjectiveQuesCorrectCount = 0;
                    zysl.ForEach(s =>
                    {
                        if (s.QuesType == 0)
                        {
                            ObjectiveQuesCorrectCount += xx.FirstOrDefault(t => t.StructId == s.Id).Answer == s.QuesAnswer ? 1 : 0;
                        }
                    });
                    dto_Answer3 da = new dto_Answer3()
                    {
                        Id = a.Id,
                        ZyId = a.ZyId,
                        StudentId = a.StudentId,
                        TrueName = a.TrueName,
                        AnswerImg = a.AnswerImg,
                        AnswerJson = a.AnswerJson,
                        CreateDate = a.CreateDate,
                        ObjectiveQuesCount = ObjectiveQuesCount,
                        ObjectiveQuesCorrectCount = ObjectiveQuesCorrectCount
                    };
                    dal.Add(da);
                }
            }
            return PartialView(dal);
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

        /// <summary>
        /// 已完成作业
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 已建作业
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 如果建作业的老师并未登录，那么认为是试用账号，其作业可以随便查看
        /// 如果建作业的老师是登录过的，那么要想查看作业，需满足条件之一：
        /// 1、查看人是新建人自己
        /// 2、查看人已经登录并且完成了此作业
        /// </summary>
        /// <param name="zy"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public string QueryAccess(string zyNum)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            T_Zy zy = B_ZyRedis.GetZy(zyId);
            if (zy.UserId == 0) return "0";
            if (zy.UserId == UserId) return "0";
            if (UserId == 0)
            {
                return "1|您尚未登录，不能查看此作业！";
            }
            if (B_ZyRedis.GetZyAnswer(zy.Id, UserId) == null)
            {
                return "1您尚未完成此作业，不能查看！";
            }
            return "0";
        }

        public ActionResult ZyChart(string zyNum)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            List<T_ZyStruct> zysl = B_ZyRedis.GetZyStruct(zyId);
            List<T_Answer> al = B_Zy.GetZyAnswers(zyId);
            int totalCount = 0;
            int totalCorrectCount = 0;
            string qno = "", cp = "";
            zysl.ForEach(a =>
            {
                if (a.QuesType == 0)
                {
                    qno += "," + a.BqNum + (a.SqNum == 0 ? "" : ("." + a.SqNum));

                    if (al != null)
                    {
                        totalCount = al.Count;
                        totalCorrectCount = 0;
                        al.ForEach(b =>
                        {
                            List<dto_Answer> ansl = JsonConvert.DeserializeObject<List<dto_Answer>>(b.AnswerJson);
                            totalCorrectCount += (ansl.FirstOrDefault(c => c.StructId == a.Id).Answer == a.QuesAnswer ? 1 : 0);
                        });
                    }
                    cp += "," + Math.Round((totalCorrectCount * 1.0 / totalCount * 1.0), 2) * 100;
                }
            });
            if (qno.Length > 0)
            {
                qno = qno.Substring(1);
                cp = cp.Substring(1);
            }
            ViewBag.TotalCount = totalCount;
            ViewBag.ZyNum = zyNum;
            ViewBag.qno = qno;
            ViewBag.cp = cp;
            return View();
        }

        /// <summary>
        /// 获取客观题选项选择率
        /// </summary>
        /// <param name="zyNum"></param>
        /// <param name="bqNum"></param>
        /// <param name="sqNum"></param>
        /// <returns></returns>
        public JsonResult GetOptionPercent(string zyNum, int bqNum, int sqNum)
        {
            int zyId = EasyzyConst.GetZyId(zyNum);
            List<T_Answer> al = B_Zy.GetZyAnswers(zyId);
            List<T_ZyStruct> zysl = B_ZyRedis.GetZyStruct(zyId);
            string qa = zysl.FirstOrDefault(a => a.BqNum == bqNum && a.SqNum == sqNum).QuesAnswer;

            int AC = 0, BC = 0, CC = 0, DC = 0;
            if (al != null)
            {
                al.ForEach(a =>
                {
                    List<dto_Answer> ansl = JsonConvert.DeserializeObject<List<dto_Answer>>(a.AnswerJson);
                    string answer = ansl.FirstOrDefault(b => b.BqNum == bqNum && b.SqNum == sqNum).Answer;
                    switch (answer)
                    {
                        case "A": AC += 1; break;
                        case "B": BC += 1; break;
                        case "C": CC += 1; break;
                        case "D": DC += 1; break;
                    }
                });
            }
            dto_Echart_pie_simple[] el = new dto_Echart_pie_simple[4];

            el[0] = new dto_Echart_pie_simple() { value = AC, name = "A" + (qa == "A" ? " 正确答案" : "") };
            el[1] = new dto_Echart_pie_simple() { value = BC, name = "B" + (qa == "B" ? " 正确答案" : "") };
            el[2] = new dto_Echart_pie_simple() { value = CC, name = "C" + (qa == "C" ? " 正确答案" : "") };
            el[3] = new dto_Echart_pie_simple() { value = DC, name = "D" + (qa == "D" ? " 正确答案" : "") };
            //string result = "{value:" + AC + ", name:'A'}|{value:" + BC + ", name:'B'}|{value:" + CC + ", name:'C'}|{value:" + DC + ", name:'D'}";

            return Json(el);
        }
    }
}