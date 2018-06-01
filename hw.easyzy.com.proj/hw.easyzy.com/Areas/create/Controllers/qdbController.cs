using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.create.Controllers
{
    public class qdbController : baseController
    {
        /// <summary>
        /// 新建页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Dictionary<int, string> pd = new Dictionary<int, string>();
            Dictionary<int, string> jd = new Dictionary<int, string>();
            Dictionary<int, string> sd = new Dictionary<int, string>();
            string c = "", d = "";

            Const.StageCourses.TryGetValue(Const.StagesEnum.PRIMARY, out c);
            foreach (var i in c.Split(','))
            {
                Const.Courses.TryGetValue(int.Parse(i), out d);
                pd.Add(int.Parse(i), d.Substring(2));
            }

            Const.StageCourses.TryGetValue(Const.StagesEnum.JUNIOR_MIDDLE, out c);
            foreach (var i in c.Split(','))
            {
                Const.Courses.TryGetValue(int.Parse(i), out d);
                jd.Add(int.Parse(i), d.Substring(2));
            }

            Const.StageCourses.TryGetValue(Const.StagesEnum.SENIOR_MIDDLE, out c);
            foreach (var i in c.Split(','))
            {
                Const.Courses.TryGetValue(int.Parse(i), out d);
                sd.Add(int.Parse(i), d.Substring(2));
            }

            ViewBag.PrimaryCourses = pd;
            ViewBag.JuniorCourses = jd; ;
            ViewBag.SeniorCourses = sd; ;
            return View();
        }

        /// <summary>
        /// 教材版本列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public JsonResult GetTextBookVersions(int courseId)
        {
            List<T_TextBookVersions> list = B_QuesBase.GetTextBookVersions(courseId);
            return Json(list);
        }

        /// <summary>
        /// 课本列表
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public JsonResult GetTextBooks(int versionId)
        {
            List<T_TextBooks> list = B_QuesBase.GetTextBooks(versionId);
            return Json(list);
        }

        /// <summary>
        /// 目录树
        /// </summary>
        /// <param name="textbookId"></param>
        /// <returns></returns>
        public JsonResult GetCatalogTree(int textbookId)
        {
            List<dto_ZTree> list = B_QuesBase.GetCatalogsTree(textbookId);
            return Json(list);
        }

        /// <summary>
        /// 知识点树
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public JsonResult GetKnowledgePointTree(int courseId)
        {
            List<dto_ZTree> list = B_QuesBase.GetKnowledgePointsTree(courseId);
            return Json(list);
        }

        /// <summary>
        /// 筛题
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="kpointId"></param>
        /// <param name="cpointId"></param>
        /// <param name="typeId"></param>
        /// <param name="diffType"></param>
        /// <param name="paperYear"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetQuestions(int courseId, int kpointId, int cpointId, int typeId, int diffType, int paperYear, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            int[] qids = B_Ques.GetQuesIds(courseId, kpointId, cpointId, typeId, diffType, paperYear, pageIndex, pageSize, out totalCount);
            int totalPage = Util.GetTotalPageCount(totalCount, pageSize);

            List<T_Questions> list = null;
            if (qids != null && qids.Length > 0)
            {
                list = new List<T_Questions>();
                foreach (var q in qids)
                {
                    T_Questions ques = B_QuesRedis.GetQuestion(q);
                    if (ques != null)
                    {
                        list.Add(ques);
                    }
                }
            }
            ViewBag.PageCount = totalPage;
            ViewBag.Ques = list;
            return PartialView();
        }

        public JsonResult GetQuesTypes(int courseId)
        {
            return Json(B_QuesBase.GetQuesTypes(courseId));
        }
    }
}