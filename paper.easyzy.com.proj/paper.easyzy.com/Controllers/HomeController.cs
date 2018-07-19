using easyzy.sdk;
using paper.easyzy.bll;
using paper.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace paper.easyzy.com.Controllers
{
    public class HomeController : Controller
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

        public ActionResult PaperInfo(long id)
        {
            ViewBag.PaperId = id;
            return View();
        }

        public ActionResult GetQuestions(long paperId)
        {
            ViewBag.QuesList = B_Paper.GetPaperQuestions(paperId);
            return PartialView();
        }
    }
}