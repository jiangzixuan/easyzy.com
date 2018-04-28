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

namespace hw.easyzy.com.Areas.submit.Controllers
{
    public class selfController : baseController
    {
        // GET: submit/self
        public ActionResult Index()
        {
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
            if (hasPsd == "0")
            {
                hasPsd = hasPsd + "|" + JsonConvert.SerializeObject(zy);
            }

            return hasPsd;
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