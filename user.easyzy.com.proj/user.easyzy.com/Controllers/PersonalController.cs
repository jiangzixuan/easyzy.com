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

            if (UserInfo.ProvinceId == 0)
            {
                ViewBag.Provinces = Const.Provinces;
            }

            if (UserInfo.CityId == 0 && UserInfo.ProvinceId != 0)
            {
                ViewBag.Cities = B_BaseRedis.GetCities(UserInfo.ProvinceId);
            }

            if (UserInfo.DistrictId == 0 && UserInfo.CityId != 0)
            {
                ViewBag.Districts = B_BaseRedis.GetDistricts(UserInfo.CityId);
            }

            if (UserInfo.SchoolId == 0 && UserInfo.DistrictId != 0)
            {
                ViewBag.Schools = B_BaseRedis.GetSchools(UserInfo.DistrictId);
            }

            if (UserInfo.GradeId == 0)
            {
                ViewBag.Grades = Const.Grades;
            }

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
            return PartialView();
        }

        public string UpdateTrueName(string trueName)
        {
            string result = "0";
            if (B_User.UpdateTrueName(UserId, trueName) > 0)
            {
                B_UserRedis.UpdateTrueName(UserId, trueName);
            }
            else
            {
                result = "1";
            }

            return result;
        }

        /// <summary>
        /// 修改默认作业密码
        /// </summary>
        /// <param name="zyPsd"></param>
        /// <returns></returns>
        public string UpdateZyPsd(string zyPsd)
        {
            string result = "0";
            if (B_User.UpdateZyPsd(UserId, zyPsd) > 0)
            {
                B_UserRedis.UpdateZyPsd(UserId, zyPsd);
            }
            else
            {
                result = "1";
            }

            return result;
        }

        /// <summary>
        /// 修改班级信息
        /// </summary>
        /// <param name="userClass"></param>
        /// <returns></returns>
        public string UpdateUserClass(string userClass)
        {
            string result = "0";
            if (B_User.UpdateClass(UserId, userClass) > 0)
            {
                B_UserRedis.ReloadUserCache(UserId);
            }
            else
            {
                result = "1";
            }

            return result;
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
            List<T_User> list = B_User.SearchUser(keyWords);
            if (list == null) return Json(new List<T_User>());
            return Json(list);
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string Relate(int userId)
        {
            if (UserId == userId) return "不能关注自己！";
            //查询是否关注过
            List<dto_RelateUser> list = B_User.GetRelateUser(UserId);
            if (list != null && list.Exists(a => a.RUserId == userId)) return "已经关注过，不能重复关注！";
            return B_User.AddRelate(UserId, userId, DateTime.Now) > 0 ? "" : "操作失败！";
        }

        public string CancelRelate(int userId)
        {
            return B_User.CancelRelate(UserId, userId) > 0 ? "" : "操作失败！";
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
            ViewBag.RTrueName = u == null ? "" : (u.UserName + "(" + u.TrueName + ")");
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView(list);
        }

        public JsonResult SearchSchools(int districtId, string keywords)
        {
            List<T_School> sl = B_BaseRedis.GetSchools(districtId);
            if (sl != null)
            {
                sl = sl.FindAll(a => a.SchoolName.Contains(keywords));
            }
            return Json(sl);
        }
    }
}