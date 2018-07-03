using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.common;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.submit.Controllers
{
    public class baseController : Controller
    {
        protected int UserId = 0;
        protected T_User UserInfo = null;

        public baseController()
        {
            string DesUserModel = Util.GetCookie("easyzy.user", "useridentity");
            string DesKey = Util.GetAppSetting("DesKey");
            UserCookieHelper.UserCookieModel u = UserCookieHelper.DescryptUserCookie(DesUserModel, DesKey);

            UserId = u._id;
            if (UserId != 0)
            {
                UserInfo = B_UserRedis.GetUser(UserId);
            }
            ViewBag.UserInfo = UserInfo;
        }

        /// <summary>
        /// 作业的访问鉴权
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected dto_AjaxJsonResult<dto_Zy> AccessJudge(int userId, dto_Zy zy)
        {
            dto_AjaxJsonResult<dto_Zy> r = new dto_AjaxJsonResult<dto_Zy>();
            //如果状态有问题，只返回少数信息给客户端
            dto_Zy zy2 = new dto_Zy() { ZyName = zy.ZyName, OpenDate = zy.OpenDate, DueDate = zy.DueDate, OpenDateStr = zy.OpenDateStr, DueDateStr = zy.DueDateStr, Status = zy.Status, Type = zy.Type };
            //打开作业时，为了能把作业内容显示出来，不在此处做试用作业判断
            //if (zy.UserId == 0)
            //{
            //    r.code = AjaxResultCodeEnum.Error;
            //    r.message = "试用作业仅用于数据展示，不允许进行操作！";
            //    r.data = zy2;
            //    return r;
            //}
            if (zy.Status == 2)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业已删除，不能打开！";
                r.data = zy2;
                return r;
            }
            if (zy.Status == 1)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业已关闭，不能提交！";
                r.data = zy2;
                return r;
            }
            if (zy.OpenDate > DateTime.Now)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业没到开放时间！";
                r.data = zy2;
                return r;
            }

            if (zy.UserId != 0 && userId == 0)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "未登录/试用状态，不能打开由正式用户创建的作业，请您先登录！";
                r.data = zy2;
                return r;
            }

            if (zy.UserId != 0 && UserInfo != null && (UserInfo.GradeId == 0 || UserInfo.ClassId == 0))
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "请先到个人中心设置自己所在的学校、年级、班级等信息！";
                r.data = zy2;
                return r;
            }

            if (zy.UserId != 0)
            {
                int[] rl = B_User.GetBeRelatedUser(zy.UserId);
                if (rl == null || !rl.Any(a => a == userId))
                {
                    r.code = AjaxResultCodeEnum.Error;
                    r.message = "您尚未关注此老师，不能打开这个作业。您可在个人中心添加关注老师！";
                    r.data = zy2;
                    return r;
                }
            }

            r.code = AjaxResultCodeEnum.Success;
            r.message = "";
            r.data = zy;
            return r;
        }
    }
}