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
    public class ZyController : CommonController
    {
        public ActionResult Add()
        {
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
            if (B_Zy.UpdateZyStructed(zyId) > 0)
            {
                B_ZyRedis.UpdateZyStructed(zyId);
            }
            return i > 0 ? "" : "Error";
        }

        public ActionResult Open()
        {
            ViewBag.UploadUrl = Util.GetAppSetting("UploadUrlPrefix");
            ViewBag.PicFunc = (int)EasyzyConst.ImgFunc.SubmitAnswer;
            return View();
        }

        /// <summary>
        /// 查看作业
        /// </summary>
        /// <param name="zyNum"></param>
        /// <returns>"null" 代表不存在，2|开头代表无权访问，"1|"代表需要密码，0|开头代表可以访问</returns>
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
            if(hasPsd == "0")
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
        /// 判断作业是否需要密码
        /// 如果作业创建人是当前登录人，不需要密码
        /// 已完成此作业，也不需要密码
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
        /// 查看作业
        /// </summary>
        /// <param name="zyNum"></param>
        /// <returns></returns>
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
                ViewBag.AnswerCard = result;
            }
            ViewBag.ZyNum = zyNum;
            ViewBag.TrueName = trueName;
            ViewBag.UploadUrl = Util.GetAppSetting("UploadUrlPrefix");
            ViewBag.BodyHtml = zy.BodyHtmlPath;
            ViewBag.AnswerHtml = zy.AnswerHtmlPath;
            
            return View();
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

        public ActionResult ZyChart()
        {
            return View();
        }
    }
}