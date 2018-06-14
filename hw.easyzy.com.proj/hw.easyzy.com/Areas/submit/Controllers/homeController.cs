using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
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

        public JsonResult GetAnswerPicList(long zyId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            T_Answer ans = B_Zy.GetZyAnswer(id, UserId);
            string[] imglist2 = null;
            if (ans != null && !string.IsNullOrEmpty(ans.AnswerImg))
            {
                string[] imglist = ans.AnswerImg.Split(',');
                imglist2 = new string[imglist.Length];
                for (int i = 0; i < imglist.Length; i++)
                {
                    imglist2[i] = Util.GetAppSetting("UploadUrlPrefix") + "/" + imglist[i];
                }
            }
            dto_AjaxJsonResult<string> r = new dto_AjaxJsonResult<string>();
            r.code = AjaxResultCodeEnum.Success;
            r.message = "";
            r.data = ((ans == null || string.IsNullOrEmpty(ans.AnswerImg)) ? "" : string.Join(",", imglist2));
            return Json(r);
        }
    }
}