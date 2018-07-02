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
    public class ZyInfoController : Controller
    {
        public ActionResult Info(long zyId)
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

            r.code = AjaxResultCodeEnum.Success;
            r.message = "";
            r.data = zy;

            return Json(r);
        }

        public ActionResult GetQuestions(int courseId, long zyId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            List<dto_Question> ql = B_ZyRedis.GetQdbZyQuestions(courseId, id);
            if (ql != null)
            {
                foreach (dto_Question q in ql)
                {
                    //隐藏真实Id
                    q.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, q.id);
                    q.id = 0;
                    if (q.Children != null && q.Children.Count > 0)
                    {
                        q.Children.ForEach(a => {
                            a.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, a.id);
                            a.id = 0;
                        });
                    }
                }
            }
            ViewBag.QuesList = ql;
            return PartialView();
        }
    }
}