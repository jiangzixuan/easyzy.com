using easyzy.sdk;
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
    [LoginFilterAttribute]
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
            ViewBag.RelateUser = list;
            //被关注列表
            List<dto_RelateUser> list2 = B_User.GetBeRelatedUser(UserId);
            if (list2 != null)
            {
                foreach (var l in list2)
                {
                    T_User u = B_UserRedis.GetUser(l.UserId);
                    l.UserName = u.UserName;
                    l.TrueName = u.TrueName;
                }
            }
            ViewBag.BeRelatedUser = list2;
            
            T_UserExtend tue = B_User.GetUserExtend(UserId);
            ViewBag.Extend = tue;

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

        public ActionResult GetCreatedZy(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_UserZy> list = B_UserZy.GetUserZy(UserId, pageIndex, pageSize, out totalCount);
            if (list != null)
            {
                list.ForEach(a => a.ZyNum = Const.GetZyNum(a.ZyId));
            }
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView(list);
        }

        public ActionResult GetSubmitedZy(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_UserZy> list = B_UserZy.GetSubmitedZy(UserId, pageIndex, pageSize, out totalCount);
            if (list != null)
            {
                list.ForEach(a => a.ZyNum = Const.GetZyNum(a.ZyId));
            }
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView(list);
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

        public string CancelRelate(int userId)
        {
            return B_User.CancelRelate(UserId, userId) ? "" : "操作失败！";
        }

        public ActionResult QueryRZy(int userId, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_UserZy> list = B_UserZy.GetUserZy(userId, pageIndex, pageSize, out totalCount);
            if (list != null)
            {
                list.ForEach(a => a.ZyNum = Const.GetZyNum(a.ZyId));
            }
            T_User u = B_UserRedis.GetUser(userId);
            ViewBag.RTrueName = u == null ? "" : (u.UserName + "【" + u.TrueName + "】");
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView(list);
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

        public int UnLock()
        {
            bool b = false;
            T_UserExtend ue = new T_UserExtend() { UserId = UserId, Locked = false };
            b = B_User.UpdateUserExtend(ue);
            
            return b ? 0 : 1;
        }
    }
}