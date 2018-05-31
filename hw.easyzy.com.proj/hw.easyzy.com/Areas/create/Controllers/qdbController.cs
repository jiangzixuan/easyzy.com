﻿using easyzy.sdk;
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

        public JsonResult GetTextBookVersions(int courseId)
        {
            List<T_TextBookVersions> list = B_QuesBase.GetTextBookVersions(courseId);
            return Json(list);
        }

        public JsonResult GetTextBooks(int versionId)
        {
            List<T_TextBooks> list = B_QuesBase.GetTextBooks(versionId);
            return Json(list);
        }

        public JsonResult GetCatalogTree(int textbookId)
        {
            List<dto_ZTree> list = B_QuesBase.GetCatalogsTree(textbookId);
            return Json(list);
        }

        public JsonResult GetKnowledgePointTree(int courseId)
        {
            List<dto_ZTree> list = B_QuesBase.GetKnowledgePointsTree(courseId);
            return Json(list);
        }
    }
}