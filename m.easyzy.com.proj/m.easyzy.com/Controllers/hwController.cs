using easyzy.sdk;
using m.easyzy.bll;
using m.easyzy.model.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m.easyzy.com.Controllers
{
    [LoginFilter]
    public class HWController : BaseController
    {
        public ActionResult MyZy()
        {
            return View();
        }

        /// <summary>
        /// 获取我新建的作业
        /// </summary>
        /// <param name="lastId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ActionResult GetMyZy(long lastId, int count)
        {
            int last = lastId == 0 ? 99999999 : IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, lastId);
            List<dto_Zy> list = B_Zy.GetZyList(UserId, last, count);
            if (list != null)
            {
                foreach (var l in list)
                {
                    //隐藏真实Id
                    l.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Zy, l.Id);
                    string subName = "";
                    Const.Subjects.TryGetValue(l.SubjectId, out subName);
                    l.SubjectName = subName;
                    l.TypeName = l.Type == 0 ? "题库" : "自传";
                    l.Id = 0;
                }
            }

            ViewBag.ZyList = list;
            return PartialView();
        }

        public ActionResult RelatedUserZy()
        {
            return View();
        }

        /// <summary>
        /// 获取关注老师的作业
        /// </summary>
        /// <param name="lastId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ActionResult GetRelatedUserZy(long lastId, int count)
        {
            int last = lastId == 0 ? 99999999 : IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, lastId);
            int[] RUsers = B_User.GetRelatedUser(UserId);
            List<dto_Zy> list = B_Zy.GetZyList(RUsers, last, count);
            if (list != null)
            {
                List<int> ids = B_Answer.GetSubmitedZyIds(UserId, list.Select(a => a.Id).ToArray());
                foreach (var l in list)
                {
                    l.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Zy, l.Id);
                    string subName = "";
                    Const.Subjects.TryGetValue(l.SubjectId, out subName);
                    l.SubjectName = subName;
                    l.TypeName = l.Type == 0 ? "题库" : "自传";
                    dto_User u = B_UserRedis.GetUser(l.UserId);
                    l.UserName = u.UserName;
                    l.TrueName = u.TrueName;
                    l.Submited = ids == null ? false : ids.Exists(a => a == l.Id);
                    //隐藏真实Id
                    l.Id = 0;
                }
            }
            ViewBag.RelateUserCount = RUsers == null ? 0 : RUsers.Length;
            ViewBag.ZyList = list;
            return PartialView();
        }

        public ActionResult SubmitedZy()
        {
            return View();
        }

        /// <summary>
        /// 获取已提交作业
        /// </summary>
        /// <param name="lastId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ActionResult GetSubmitedZy(long lastId, int count)
        {
            int last = lastId == 0 ? 99999999 : IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, lastId);
            List<dto_Zy> list = null;
            List<int> ids = B_Answer.GetSubmitedZyIds(UserId, last, count);
            if (ids != null)
            {
                list = new List<dto_Zy>();
                ids.ForEach(a =>
                {
                    list.Add(B_ZyRedis.GetZy(a));
                });
            }
            if (list != null)
            {
                foreach (var l in list)
                {
                    //隐藏真实Id
                    l.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Zy, l.Id);

                    dto_User u = B_UserRedis.GetUser(l.UserId);
                    l.UserName = u.UserName;
                    l.TrueName = u.TrueName;
                    l.Id = 0;
                }
            }

            ViewBag.ZyList = list;
            return PartialView();
        }
    }
}