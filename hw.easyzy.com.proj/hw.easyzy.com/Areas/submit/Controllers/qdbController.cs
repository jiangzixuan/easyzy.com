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
            dto_AjaxJsonResult<dto_Zy> r1 = AccessJudge(UserId, zy);
            if (r1.code == AjaxResultCodeEnum.Error)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = r1.message;
                r.data = "";
                return JsonConvert.SerializeObject(r);
            }

            //作业状态验证
            T_Answer ans = B_Answer.GetAnswer(id, UserId);
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
                    isok = B_Answer.InsertZyAnswer(AnsAdd);
                }
                else
                {
                    isok = B_Answer.AddZyImg(zy.Id, UserId, r.data);
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
            if (string.IsNullOrEmpty(questions))
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "提交试题信息有误！";
                r.data = "";
                return Json(r);
            }

            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);
            #region 验证
            //访问权限验证
            dto_AjaxJsonResult<dto_Zy> r1 = AccessJudge(UserId, zy);
            if (r1.code == AjaxResultCodeEnum.Error)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = r1.message;
                r.data = "";
                return Json(r);
            }

            //试用作业允许多次提交
            T_Answer ans = B_Answer.GetAnswer(id, UserId);
            
            if (zy.UserId != 0 && ans != null && ans.Submited)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业已提交，不能重复提交！";
                r.data = "";
                return Json(r);
            }
            
            #endregion

            //todo submit
            List<string> submitQlist = questions.Split(',').ToList();
            List<string> submitAlist = string.IsNullOrEmpty(answers) ? new List<string>() : answers.Split(',').ToList();
            if (submitQlist.Count != submitAlist.Count)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "试题信息有误，提交失败！";
                r.data = "";
                return Json(r);
            }
            string qjson = B_ZyRedis.GetQdbZyQuesJson(id);
            List<dto_ZyQuestion> ql = JsonConvert.DeserializeObject<List<dto_ZyQuestion>>(qjson);
            List<dto_UserAnswer> al = new List<dto_UserAnswer>();
            ql.ForEach(a => {
                string CAnswer = "";
                if (Const.OBJECTIVE_QUES_TYPES.Contains(a.PTypeId))
                {
                    CAnswer = B_QuesRedis.GetQuestion(zy.CourseId, a.QId).quesanswer;
                }
                int i = submitQlist.IndexOf(IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, a.QId).ToString());
                al.Add(new dto_UserAnswer() { QId = a.QId, PTypeId = a.PTypeId, Score = a.Score, Answer = (i == -1 ? "" : submitAlist[i]), CAnswer = CAnswer, Point = 0 });
            });
            bool isok = false;
            if (zy.UserId != 0 && ans != null)
            {
                isok = B_Answer.UpdateAnswerJson(id, UserId, JsonConvert.SerializeObject(al));
            }
            else
            {
                T_Answer answer = new T_Answer()
                {
                    ZyId = id,
                    ZyType = zy.Type,
                    StudentId = UserId,
                    Submited = true,
                    CreateDate = DateTime.Now,
                    AnswerJson = JsonConvert.SerializeObject(al),
                    AnswerImg = "",
                    Ip = ClientUtil.Ip,
                    IMEI = ClientUtil.IMEI,
                    MobileBrand = ClientUtil.MobileBrand,
                    SystemType = Request.Browser.Platform.ToString(),
                    Browser = Request.Browser.Browser.ToString()
                };
                isok = B_Answer.InsertZyAnswer(answer);
            }
            if (isok)
            {
                r.code = AjaxResultCodeEnum.Success;
                r.message = "";
                r.data = "";
                return Json(r);
            }
            else
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "提交入库失败！";
                r.data = "";
                return Json(r);
            }
        }
    }
}