﻿using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using user.easyzy.bll;
using user.easyzy.model.dto;
using user.easyzy.model.entity;

namespace user.easyzy.com.Controllers
{
    [LoginFilter]
    public class PersonalController : BaseController
    {
        public ActionResult Index()
        {
            if (UserId == 0)
            {
                return View("login");
            }
            return View();
        }

        public ActionResult GetUserInfo()
        {
            dto_User UserInfo = B_UserRedis.GetUser(UserId);
            ViewBag.UserInfo = UserInfo;

            if (UserInfo.SchoolId == 0)
            {
                ViewBag.Provinces = Const.Provinces;
            }

            ViewBag.Grades = Const.Grades;

            return PartialView();
        }

        public int UpdateTrueName(string trueName)
        {
            if (B_User.UpdateTrueName(UserId, trueName))
            {
                B_UserRedis.UpdateTrueName(UserId, trueName);
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 修改学校信息，如果已经被改过，则不能重复修改
        /// </summary>
        /// <param name="userClass"></param>
        /// <returns></returns>
        public int UpdateUserSchool(int provinceId, int cityId, int districtId, int schoolId)
        {
            if (provinceId == 0 || cityId == 0 || districtId == 0 || schoolId == 0)
            {
                return 1;
            }
            dto_User du = B_UserRedis.GetUser(UserId);
            if (du.SchoolId != 0)
            {
                return 1;
            }
            if (B_User.UpdateSchool(UserId, provinceId, cityId, districtId, schoolId))
            {
                B_UserRedis.ReloadUserCache(UserId);
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 修改班级信息
        /// </summary>
        /// <param name="gradeId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public int UpdateUserClass(int gradeId, int classId)
        {
            if (gradeId == 0 || classId == 0)
            {
                return 1;
            }

            if (B_User.UpdateClass(UserId, gradeId, classId))
            {
                string gName = "";
                Const.Grades.TryGetValue(gradeId, out gName);
                gName = gName == null ? "" : gName;

                B_UserRedis.UpdateClass(UserId, gradeId, gName, classId, classId + "班");
                return 0;
            }
            return 1;
        }

        public JsonResult GetCites(int provinceId)
        {
            List<T_City> cl = B_BaseRedis.GetCities(provinceId);
            return Json(cl);
        }

        public JsonResult GetDistricts(int cityId)
        {
            List<T_District> dl = B_BaseRedis.GetDistricts(cityId);
            return Json(dl);
        }

        /// <summary>
        /// 我的关注
        /// </summary>
        /// <returns></returns>
        public ActionResult MyRelation()
        {
            //关注列表
            List<dto_RelateUser> list = B_User.GetRelateUser(UserId);
            if (list != null)
            {
                foreach (var l in list)
                {
                    T_User u = B_UserRedis.GetUser(l.RUserId);
                    l.RUserName = u.UserName;
                    l.RTrueName = u.TrueName;
                }
            }
            ViewBag.List = list;
            return PartialView();
        }

        /// <summary>
        /// 关注我的
        /// </summary>
        /// <returns></returns>
        public ActionResult RelateMe()
        {
            //被关注列表
            List<dto_RelateUser> list = B_User.GetBeRelatedUser(UserId);
            if (list != null)
            {
                foreach (var l in list)
                {
                    T_User u = B_UserRedis.GetUser(l.UserId);
                    l.UserName = u.UserName;
                    l.TrueName = u.TrueName;
                }
            }
            ViewBag.List = list;

            T_UserExtend tue = B_User.GetUserExtend(UserId);
            ViewBag.Extend = tue;
            return PartialView();
        }

        public JsonResult SearchUser(string keyWords)
        {
            List<T_User> list = B_User.SearchUser(keyWords, UserId);
            if (list == null) return Json(new List<dto_User>());
            int[] uIds = list.Select(a => a.Id).ToArray();
            List<T_UserExtend> uel = B_User.GetUserExtends(uIds);

            List<dto_User> ul = new List<dto_User>();
            foreach (var l in list)
            {
                string gName = "";
                Const.Grades.TryGetValue(l.GradeId, out gName);
                T_UserExtend ue = uel == null ? null : uel.Find(a => a.UserId == l.Id);
                ul.Add(new dto_User()
                {
                    Id = l.Id,
                    UserName = l.UserName,
                    TrueName = l.TrueName,
                    SchoolName = B_Base.GetSchool(l.SchoolId).SchoolName,
                    GradeName = gName == null ? "" : gName,
                    ClassName = l.ClassId + "班",
                    Locked = ue == null ? false : ue.Locked
                });
            }
            return Json(ul);
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string Relate(int userId)
        {
            if (UserId == userId) return "不能关注自己！";
            //查询用户是否已经锁定
            T_UserExtend tue = B_User.GetUserExtend(userId);
            if (tue != null && tue.Locked) return "用户已经锁定，不能关注！";
            //查询是否关注过
            List<dto_RelateUser> list = B_User.GetRelateUser(UserId);
            if (list != null && list.Exists(a => a.RUserId == userId)) return "已经关注过，不能重复关注！";

            return B_User.AddRelate(UserId, userId, DateTime.Now) ? "" : "操作失败！";
        }

        /// <summary>
        /// 取消关注某人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string CancelRelate(int userId)
        {
            return B_User.CancelRelate(UserId, userId) ? "" : "操作失败！";
        }
        
        public JsonResult SearchSchools(int districtId, string keywords)
        {
            List<T_School> sl = B_BaseRedis.GetSchools(districtId);
            if (!string.IsNullOrEmpty(keywords))
            {
                if (sl != null)
                {
                    sl = sl.FindAll(a => a.SchoolName.Contains(keywords));
                }
            }
            return Json(sl);
        }

        public ActionResult RequestModify()
        {
            dto_ModifyRequest drm = B_User.GetModifyRequest(UserId);
            if (drm != null)
            {
                drm.FromSchoolName = B_BaseRedis.GetSchool(drm.FromSchoolId).SchoolName;
                drm.ToSchoolName = B_BaseRedis.GetSchool(drm.ToSchoolId).SchoolName;
            }
            ViewBag.Model = drm;
            ViewBag.Provinces = Const.Provinces;
            return PartialView();
        }

        public int CancelModifyRequest(int id)
        {
            if (B_User.CancelModifyRequest(id))
            {
                return 0;
            }
            return 1;
        }

        public JsonResult AddModifyRequest(int schoolId, string reason)
        {
            dto_User u = B_UserRedis.GetUser(UserId);

            dto_ModifyRequest mr = new dto_ModifyRequest() { UserId = UserId, FromSchoolId = u.SchoolId, ToSchoolId = schoolId, Reason = reason, CreateDate = DateTime.Now, Status = 0 };
            int i = B_User.AddModifyRequest(mr);
            if (i != 0)
            {
                mr.Id = i;
                mr.CreateDateStr = mr.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                mr.FromSchoolName = B_BaseRedis.GetSchool(mr.FromSchoolId).SchoolName;
                mr.ToSchoolName = B_BaseRedis.GetSchool(mr.ToSchoolId).SchoolName;
                return Json(mr);
            }
            return null;
        }

        /// <summary>
        /// 锁定
        /// </summary>
        /// <returns></returns>
        public int Lock()
        {
            T_UserExtend ue = B_User.GetUserExtend(UserId);
            bool b = false;
            if (ue == null)
            {
                ue = new T_UserExtend() { UserId = UserId, Locked = true };
                b = B_User.AddUserExtend(ue);
            }
            else
            {
                ue.Locked = true;
                b = B_User.UpdateUserExtend(ue);
            }
            return b ? 0 : 1;
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <returns></returns>
        public int UnLock()
        {
            bool b = false;
            T_UserExtend ue = new T_UserExtend() { UserId = UserId, Locked = false };
            b = B_User.UpdateUserExtend(ue);
            
            return b ? 0 : 1;
        }

        /// <summary>
        /// 删除关注我的人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult DeleteRelate(int userId)
        {
            dto_AjaxJsonResult<bool> result = new dto_AjaxJsonResult<bool>();
            bool b = B_User.DeleteRelate(UserId, userId);
            if (b)
            {
                result.code = AjaxResultCodeEnum.Success;
                result.message = "";
                result.data = true;
            }
            else
            {
                result.code = AjaxResultCodeEnum.Error;
                result.message = "删除失败！";
                result.data = false;
            }
            return Json(result);
        }

        /// <summary>
        /// 本班同学
        /// </summary>
        /// <returns></returns>
        public ActionResult MyClassmates()
        {
            dto_User u = B_UserRedis.GetUser(UserId);
            List<T_User> list = B_User.GetClassmates(u.SchoolId, u.GradeId, u.ClassId);
            ViewBag.List = list;
            return PartialView();
        }
    }
}