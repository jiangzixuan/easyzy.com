using easyzy.sdk;
using paper.easyzy.bll;
using paper.easyzy.model.dto;
using paper.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace paper.easyzy.com.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.PrimaryCourses = B_Common.GetCoursesByStage(Const.StagesEnum.PRIMARY);
            ViewBag.JuniorCourses = B_Common.GetCoursesByStage(Const.StagesEnum.JUNIOR_MIDDLE);
            ViewBag.SeniorCourses = B_Common.GetCoursesByStage(Const.StagesEnum.SENIOR_MIDDLE);

            ViewBag.PaperTypes = B_Common.GetPaperTypes();
            ViewBag.Provinces = B_Common.GetProvinces();

            return View();
        }

        public ActionResult SearchPapers(int courseId, int gradeId, int typeId, int paperYear, int areaId, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_Paper> list = B_Paper.SearchPapers(courseId, gradeId, typeId, paperYear, areaId, pageIndex, pageSize, out totalCount);
            List<dto_Paper> pl = B_Paper.SetPaperSubmited(list, UserId);
            ViewBag.PaperList = B_Paper.ResetPaperId(pl);
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }

        //[LoginFilterAttribute]
        public ActionResult PaperInfo(long id)
        {
            dto_Paper p = B_Paper.GetPaper(id);
            bool b = B_Paper.IsPaperSubmited(id, UserId);
            ViewBag.CourseId = p == null ? 0 : p.CourseId;
            ViewBag.PaperName = p == null ? "" : p.Title;
            ViewBag.PaperId = id;
            ViewBag.IsSubmited = b;
            return View();
        }

        //[LoginFilterAttribute]
        public ActionResult GetQuestions(int courseId, long paperId)
        {
            List<dto_Question> list = B_Paper.GetPaperQuestions(courseId, paperId);
            if (list != null)
            {
                foreach (var l in list)
                {
                    l.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, l.id);
                    l.id = 0;
                    if (l.Children != null && l.Children.Count > 0)
                    {
                        l.Children.ForEach(a =>
                        {
                            a.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, a.id);
                            a.id = 0;
                        });
                    }
                }
            }
            ViewBag.QuesList = list;
            return PartialView();
        }

        /// <summary>
        /// 提交答案
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="questions"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        //[LoginFilterAttribute]
        public JsonResult SubmitAnswer(int courseId, long paperId, string questions, string answers)
        {
            dto_AjaxJsonResult<string> r = new dto_AjaxJsonResult<string>();
            string SystemType = Request.Browser.Platform.ToString();
            string Browser = Request.Browser.Browser.ToString();

            string result = B_Paper.SubmitAnswer(courseId, paperId, UserId, questions, answers, SystemType, Browser);
            if (string.IsNullOrEmpty(result))
            {
                r.code = AjaxResultCodeEnum.Success;
                r.message = "";
                r.data = "";
            }
            else
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = result;
                r.data = "";
            }
            return Json(r);
        }

        //[LoginFilterAttribute]
        public ActionResult GetPaperAnswer(int courseId, long paperId)
        {
            ViewBag.QuesList = B_Paper.GetPaperAnswer(courseId, paperId, UserId);
            return PartialView();
        
        }

        /// <summary>
        /// 将T_Paper表的QuestionIds修改为T_Questions表的Id字符串，不是业务用到的Controller方法
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ActionResult ModifyPaperQuestions(string courseid)
        {
            string[] cl = courseid.Split(',');
            foreach (var c in cl)
            {
                B_Paper.ModifyPaperQuestions(int.Parse(c));
            }
            return View();
        }
    }
}