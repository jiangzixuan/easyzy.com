using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.list.Controllers
{
    /// <summary>
    /// 已完成作业
    /// </summary>
    [LoginFilterAttribute]
    public class submitedController : baseController
    {
        public ActionResult Desc(long zyId, int studentId = 0)
        {
            if (studentId == 0)
            {
                studentId = UserId;
            }
            ViewBag.StudentId = studentId;
            ViewBag.ZyId = zyId;
            return View();
        }

        public ActionResult GetQuestionAndAnswers(int courseId, long zyId, int studentId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);

            dto_Zy zy = B_ZyRedis.GetZy(id);
            T_Answer answer = null;
            if (zy.UserId == 0)
            {
                return PartialView();
            }
            else
            {
                answer = B_Answer.GetAnswer(id, (studentId == 0 ? UserId : studentId));
            }
            List<dto_UserAnswer> ansl = null;
            if (answer != null)
            {
                ViewBag.PicPrefix = Util.GetAppSetting("UploadUrlPrefix") + "/";
                ViewBag.AnswerImg = answer.AnswerImg;
                ansl = JsonConvert.DeserializeObject<List<dto_UserAnswer>>(answer.AnswerJson);
            }

            List<dto_Question> ql = B_ZyRedis.GetQdbZyQuestions(courseId, id);
            if (ql != null)
            {
                foreach (dto_Question q in ql)
                {
                    if (!q.haschildren && Const.OBJECTIVE_QUES_TYPES.Contains(q.ptypeid))
                    {
                        q.SAnswer = ansl == null ? "" : ansl.Find(b => b.QId == q.id).Answer;
                    }
                    //隐藏真实Id
                    q.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, q.id);
                    q.id = 0;

                    if (q.Children != null && q.Children.Count > 0)
                    {
                        q.Children.ForEach(a => {
                            if (Const.OBJECTIVE_QUES_TYPES.Contains(a.ptypeid))
                            {
                                a.SAnswer = ansl == null ? "" : ansl.Find(b => b.QId == a.id).Answer;
                            }
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