using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.analyze.Controllers
{
    public class homeController : baseController
    {
        public ActionResult Index(long zyId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);
            ViewBag.ZyId = zyId;
            List<dto_ClassSubmitCount> Classes = B_Analyze.GetSubmitClasses(id);
            if (Classes != null)
            {
                Classes.ForEach(a =>
                {
                    if (a.SchoolId == 0 && a.GradeId == 0 && a.ClassId == 0)
                    {
                        a.SchoolName = "试用学校";
                        a.GradeName = "试用年级";
                        a.ClassName = "试用班";
                    }
                    else
                    {
                        a.SchoolName = B_BaseRedis.GetSchool(a.SchoolId).SchoolName;
                        a.GradeName = Const.Grades[a.GradeId];
                        a.ClassName = a.ClassId + "班";
                    }
                });
            }
            ViewBag.Classes = Classes;
            ViewBag.ClassCount = Classes == null ? 0 : Classes.Count;
            return View(zy);
        }

        public ActionResult GetClassBar(long zyId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Echart_Bar3 deb = null;

            List<dto_ClassSubmitCount> Classes = B_Analyze.GetSubmitClasses(id);
            if (Classes != null)
            {
                deb = new dto_Echart_Bar3();
                deb.data = new List<dto_Echart_Bar3_Data>();
                dto_Echart_Bar3_Data debd = null;
                foreach (var c in Classes)
                {
                    debd = new dto_Echart_Bar3_Data();
                    string cname = "";
                    if (c.SchoolId == 0 && c.GradeId == 0 && c.ClassId == 0)
                    {
                        cname = "试用学校试用班";
                    }
                    else
                    {
                        cname = Const.Grades[c.GradeId] + c.ClassId + "班";
                    }
                    debd.name = cname;
                    debd.value = c.SubmitCount;
                    deb.data.Add(debd);
                }
            }
            ViewBag.xData = string.Join(",", deb.data.Select(a=>a.name).ToArray());
            ViewBag.yData = JsonConvert.SerializeObject(deb.data);
            ViewBag.ClassCount = Classes.Count;
            ViewBag.StuCount = Classes.Sum(a => a.SubmitCount);
            return PartialView();
        }

        /// <summary>
        /// 提交统计
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public ActionResult GetSubmitBar(long zyId, int schoolId, int gradeId, int classId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            
            dto_Echart_Bar deb = B_Analyze.GetStudentPoint(id, schoolId, gradeId, classId);
            if (deb != null)
            {
                for (int i = 0; i< deb.x.Count; i++)
                {
                    if (deb.x[i] == "0")
                    {
                        deb.x[i] = "试用学生";
                    }
                    else
                    {
                        deb.x[i] = B_UserRedis.GetUser(int.Parse(deb.x[i])).TrueName;
                    }
                }
                
                ViewBag.xData = string.Join(",", deb.x);
                ViewBag.yData = string.Join(",", deb.y);
            }
            int ObjectiveCount = JsonConvert.DeserializeObject<List<dto_ZyQuestion>>(B_ZyRedis.GetQdbZyQuesJson(id)).Count(a => Const.OBJECTIVE_QUES_TYPES.Contains(a.PTypeId));
            ViewBag.ObjectiveCount = ObjectiveCount;
            int SubmitCount = deb == null ? 0 : deb.x.Count;
            ViewBag.SubmitCount = SubmitCount;
            double ScoreRate = 0;
            ScoreRate = (SubmitCount == 0 || ObjectiveCount == 0 || deb == null) ? 0 : Math.Round((deb.y.Sum(a => int.Parse(a)) * 1.0 / (ObjectiveCount * SubmitCount)), 4) * 100;
            ViewBag.ScoreRate = ScoreRate;
            dto_Zy zy = B_ZyRedis.GetZy(id);
            ViewBag.InTime = deb == null ? 0 : deb.o.Count(a => a <= zy.DueDate);
            ViewBag.OverTime = deb == null ? 0 : deb.o.Count(a => a > zy.DueDate);
            return PartialView();
        }

        /// <summary>
        /// 试题统计
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public ActionResult GetQuesBar(long zyId, int schoolId, int gradeId, int classId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);

            dto_Echart_Bar deb = B_Analyze.GetQuesCorrectCount(id, schoolId, gradeId, classId);
            if (deb != null)
            {
                ViewBag.xData = string.Join(",", deb.x);
                ViewBag.yData = string.Join(",", deb.y);
            }

            ViewBag.ObjectiveCount = deb == null ? 0 : deb.x.Count;
            ViewBag.Worst = deb == null ? "" : deb.x[deb.y.IndexOf(deb.y.Min(a => a))];
            return PartialView();
        }

        /// <summary>
        /// 选项统计
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="schoolId"></param>
        /// <param name="gradeId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public ActionResult GetOptionBar(long zyId, int schoolId, int gradeId, int classId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Echart_Bar2 deb = B_Analyze.GetOptionSelectCount(id, schoolId, gradeId, classId);
            if (deb != null)
            {
                ViewBag.Category = string.Join(",", deb.category);
                ViewBag.AData = string.Join(",", deb.optiona);
                ViewBag.BData = string.Join(",", deb.optionb);
                ViewBag.CData = string.Join(",", deb.optionc);
                ViewBag.DData = string.Join(",", deb.optiond);
            }
            ViewBag.SubmitCount = B_Analyze.GetZySubmitCount(id, schoolId, gradeId, classId);
            return PartialView();
        }
    }
}