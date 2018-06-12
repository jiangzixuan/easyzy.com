using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.submit.Controllers
{
    public class homeController : baseController
    {
        public ActionResult Index(long zyId)
        {
            ViewBag.ZyId = zyId;
            return View();
        }

        public JsonResult GetZyInfo(long zyId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);
            zy.Id = 0;  //隐藏真实Id
            dto_AjaxJsonResult<dto_Zy> r = new dto_AjaxJsonResult<dto_Zy>();

            dto_AjaxJsonResult<int> r1 = AccessJudge(UserId, zy);
            r.code = r1.code;
            r.message = r1.message;
            r.data = zy;
            return Json(r);
        }

        /// <summary>
        /// 鉴权
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private dto_AjaxJsonResult<int> AccessJudge(int userId, dto_Zy zy)
        {
            dto_AjaxJsonResult<int> r = new dto_AjaxJsonResult<int>();
            if (zy.OpenDate > DateTime.Now)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业没到开放时间";
                r.data = 0;
                return r;
            }

            if (zy.UserId != 0 && userId == 0)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "未登录/试用状态，不能打开此作业。";
                r.data = 0;
                return r;
            }

            if (zy.UserId != 0)
            {
                List<dto_RelateUser> rl = B_User.GetBeRelatedUser(zy.UserId);
                if (rl == null || !rl.Exists(a => a.UserId == userId))
                {
                    r.code = AjaxResultCodeEnum.Error;
                    r.message = "您尚未关注此老师，不能打开此作业。";
                    r.data = 0;
                    return r;
                }
            }

            r.code = AjaxResultCodeEnum.Success;
            r.message = "";
            r.data = 0;
            return r;
        }
    }
}