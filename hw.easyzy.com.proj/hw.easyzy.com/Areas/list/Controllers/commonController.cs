using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.list.Controllers
{
    /// <summary>
    /// 通用方法
    /// </summary>
    public class commonController : baseController
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

        /// <summary>
        /// 关闭作业
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public JsonResult CloseZy(long zyId)
        {
            dto_AjaxJsonResult<string> r = new dto_AjaxJsonResult<string>();
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);
            if (zy.UserId == 0)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "试用作业仅用于数据展示，不允许进行操作！";
                r.data = "";
                return Json(r);
            }
            if (zy.Status == 2)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业已删除，不能关闭！";
                r.data = "";
                return Json(r);
            }
            else if (zy.Status == 1)
            {
                r.code = AjaxResultCodeEnum.Success;
                r.message = "";
                r.data = "";
                return Json(r);
            }
            else
            {
                bool isok = B_Zy.UpdateZyStatus(id, 1);
                if (isok)
                {
                    B_ZyRedis.UpdateZyStatus(id, 1);
                    r.code = AjaxResultCodeEnum.Success;
                    r.message = "";
                    r.data = "";
                    return Json(r);
                }
                else
                {
                    r.code = AjaxResultCodeEnum.Error;
                    r.message = "关闭失败！";
                    r.data = "";
                    return Json(r);
                }
            }
        }

        /// <summary>
        /// 删除作业
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public JsonResult DeleteZy(long zyId)
        {
            dto_AjaxJsonResult<string> r = new dto_AjaxJsonResult<string>();
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);
            if (zy.UserId == 0)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "试用作业仅用于数据展示，不允许进行操作！";
                r.data = "";
                return Json(r);
            }
            if (zy.Status == 2)
            {
                r.code = AjaxResultCodeEnum.Success;
                r.message = "";
                r.data = "";
                return Json(r);
            }
            else
            {
                bool isok = B_Zy.UpdateZyStatus(id, 2);
                if (isok)
                {
                    B_ZyRedis.UpdateZyStatus(id, 2);
                    r.code = AjaxResultCodeEnum.Success;
                    r.message = "";
                    r.data = "";
                    return Json(r);
                }
                else
                {
                    r.code = AjaxResultCodeEnum.Error;
                    r.message = "删除失败！";
                    r.data = "";
                    return Json(r);
                }
            }
        }
    }
}