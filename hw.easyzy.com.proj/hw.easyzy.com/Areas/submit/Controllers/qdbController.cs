using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.common;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.submit.Controllers
{
    public class qdbController : baseController
    {
        public ActionResult Index()
        {
            return View();
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
        /// 上传答案图片
        /// 因为ajaxfileupload限制，这里不能返回json，所以特殊处理返回string
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public string UploadAnswerImage(long zyId)
        {
            dto_AjaxJsonResult<string> r = new dto_AjaxJsonResult<string>();

            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);
            //访问权限验证
            dto_AjaxJsonResult<int> r1 = AccessJudge(UserId, zy);
            if (r1.code == AjaxResultCodeEnum.Error)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = r1.message;
                r.data = "";
                return JsonConvert.SerializeObject(r);
            }

            //作业状态验证
            T_Answer ans = B_Zy.GetZyAnswer(id, UserId);
            if (ans != null && ans.Submited)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业已提交，不能再上传答案图片！";
                r.data = "";
                return JsonConvert.SerializeObject(r);
            }

            //上传
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            r = ZyImgUploader.Upload(files);
            bool isok = false;
            if (r.code == AjaxResultCodeEnum.Success)
            {
                //入库
                if (ans == null)
                {
                    T_Answer AnsAdd = new T_Answer()
                    {
                        ZyId = zy.Id,
                        ZyType = zy.Type,
                        StudentId = UserId,
                        Submited = false,
                        CreateDate = DateTime.Now,
                        AnswerJson = "",
                        AnswerImg = r.data,
                        Ip = ClientUtil.Ip,
                        IMEI = ClientUtil.IMEI,
                        MobileBrand = ClientUtil.MobileBrand,
                        SystemType = Request.Browser.Platform.ToString(),
                        Browser = Request.Browser.Browser.ToString()
                    };
                    isok = B_Zy.AddZyAnswer(AnsAdd);
                }
                else
                {
                    isok = B_Zy.AddZyImg(zy.Id, UserId, r.data);
                }
            }
            if (!isok)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "入库失败！";
                r.data = "";
                return JsonConvert.SerializeObject(r);
            }
            r.data = Util.GetAppSetting("UploadUrlPrefix") + "/" + r.data;
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 提交答案
        /// 仍然做各种状态判断
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="questions"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        public JsonResult SubmitAnswer(long zyId, string questions, string answers)
        {
            dto_AjaxJsonResult<string> r = new dto_AjaxJsonResult<string>();

            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);

            //访问权限验证
            dto_AjaxJsonResult<int> r1 = AccessJudge(UserId, zy);
            if (r1.code == AjaxResultCodeEnum.Error)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = r1.message;
                r.data = "";
                return Json(r);
            }

            //作业状态验证
            if (zy.Status == 1)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业已关闭，不能提交！";
                r.data = "";
                return Json(r);
            }
            
            T_Answer ans = B_Zy.GetZyAnswer(id, UserId);
            if (ans != null && ans.Submited)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业已提交，不能重复提交！";
                r.data = "";
                return Json(r);
            }
            //todo submit

            return null;

        }


    }
}