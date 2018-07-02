using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.list.Controllers
{
    public class homeController : baseController
    {
        public ActionResult Index()
        {
            ViewBag.UserId = UserId;
            return View();
        }

        /// <summary>
        /// 获取试用的作业列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetTrailZy(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_Zy> list = B_Zy.GetZyList(0, pageIndex, pageSize, out totalCount);
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
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }

        /// <summary>
        /// 获取我新建的作业列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult GetMyZy(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_Zy> list = B_Zy.GetZyList(UserId, pageIndex, pageSize, out totalCount);
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
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }

        /// <summary>
        /// 获取关注老师的作业
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult GetRelatedUserZy(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            
            int[] RUsers = B_User.GetRelatedUser(UserId);
            List<dto_Zy>  list = B_Zy.GetZyList(RUsers, pageIndex, pageSize, out totalCount);
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
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }

        /// <summary>
        /// 获取已提交作业
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult GetSubmitedZy(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_Zy> list = null;
            List<int> ids = B_Answer.GetSubmitedZyIds(UserId, pageIndex, pageSize, out totalCount);
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
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }
    }
}