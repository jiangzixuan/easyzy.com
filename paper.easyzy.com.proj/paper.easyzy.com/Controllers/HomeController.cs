using easyzy.sdk;
using paper.easyzy.bll;
using paper.easyzy.model.dto;
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
            ViewBag.PaperList = B_Paper.SearchPapers(courseId, gradeId, typeId, paperYear, areaId, pageIndex, pageSize, out totalCount);
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }

        [LoginFilterAttribute]
        public ActionResult PaperInfo(long id)
        {
            dto_Paper p = B_Paper.GetPaper(id);
            ViewBag.CourseId = p == null ? 0 : p.CourseId;
            ViewBag.PaperName = p == null ? "" : p.Title;
            ViewBag.PaperId = id;
            return View();
        }

        [LoginFilterAttribute]
        public ActionResult GetQuestions(int courseId, long paperId)
        {
            ViewBag.QuesList = B_Paper.GetPaperQuestions(courseId, paperId);
            return PartialView();
        }

        /// <summary>
        /// 将T_Paper表的QuestionIds修改为T_Questions表的Id数组json，不是业务用到的Controller方法
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