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

            List<dto_Question> list = null;
            if (qids != null && qids.Length > 0)
            {
                list = new List<dto_Question>();
                foreach (var q in qids)
                {
                    dto_Question ques = B_QuesRedis.GetQuestion(courseId, q);
                    if (ques != null)
                    {
                        //暴露的qid重写
                        ques.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, ques.id);
                        ques.id = 0;
                        if (ques.Children != null && ques.Children.Count > 0)
                        {
                            ques.Children.ForEach(a => {
                                a.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, a.id);
                                a.id = 0;
                            });
                        }
                        list.Add(ques);
                    }
                }
            }
            ViewBag.PageCount = totalPage;
            ViewBag.QuesList = list;
            return PartialView();
        }

        /// <summary>
        /// 随机出题（因功能废弃暂未实现主客观题2：1比例）
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="kpointId"></param>
        /// <param name="cpointId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ActionResult RandomQuestions(int courseId, int kpointId, int cpointId, int count)
        {
            List<dto_Question> list = null;
            if (count <= 20)
            {
                int[] qids = B_Ques.GetQuesIds(courseId, kpointId, cpointId, count);
                
                if (qids != null && qids.Length > 0)
                {
                    list = new List<dto_Question>();
                    foreach (var q in qids)
                    {
                        dto_Question ques = B_QuesRedis.GetQuestion(courseId, q);
                        if (ques != null)
                        {
                            //暴露的qid重写
                            ques.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, ques.id);
                            ques.id = 0;
                            if (ques.Children != null && ques.Children.Count > 0)
                            {
                                ques.Children.ForEach(a => {
                                    a.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, a.id);
                                    a.id = 0;
                                });
                            }
                            list.Add(ques);
                        }
                    }
                }
            }

            ViewBag.QuesList = list;
            return PartialView();
        }

        public JsonResult GetQuesTypes(int courseId)
        {
            return Json(B_QuesBase.GetQuesTypes(courseId));
        }

        public ActionResult Preview(int courseId)
        {
            ViewBag.CourseId = courseId;
            string courseName = "";
            Const.Courses.TryGetValue(courseId, out courseName);
            courseName = string.IsNullOrEmpty(courseName) ? "" : courseName.Substring(2);
            ViewBag.DefaultZyName = DateTime.Now.ToString("yyyy-MM-dd") + courseName + "作业";
            DateTime now = DateTime.Now;
            ViewBag.StartDate = now.ToString("yyyy-MM-dd HH:mm");
            ViewBag.EndDate = now.AddDays(1).ToString("yyyy-MM-dd HH:mm");
            return View();
        }

        public ActionResult GetBasketQues(int courseId, string qid)
        {
            List<dto_Question> dql = null;
            if (!string.IsNullOrEmpty(qid))
            {
                dql = new List<dto_Question>();
                string[] ql = qid.Split(',');
                foreach (string q in ql)
                {
                    dto_Question dq = B_QuesRedis.GetQuestion(courseId, IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Ques, long.Parse(q)));
                    dq.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, dq.id);
                    dq.id = 0;
                    dql.Add(dq);
                }
                dql = dql.OrderBy(a => a.typeid).ToList();
            }
            ViewBag.QuesList = dql;
            return PartialView();
        }

        /// <summary>
        /// 保存作业
        /// </summary>
        /// <param name="zyName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="questions"></param>
        /// <returns></returns>
        public JsonResult SaveZy(int courseId, string zyName, string startDate, string endDate, string questions)
        {
            #region 参数验证
            dto_AjaxJsonResult<string> result = new dto_AjaxJsonResult<string>();
            if (zyName.Length > 30)
            {
                result.code = AjaxResultCodeEnum.Error;
                result.message = "作业名称不能超过30个字！";
                return Json(result);
            }
            DateTime OpenDate, DueDate;
            if (!DateTime.TryParse(startDate, out OpenDate) || !DateTime.TryParse(endDate, out DueDate))
            {
                result.code = AjaxResultCodeEnum.Error;
                result.message = "开始时间和结束时间都要设置！";
                return Json(result);
            }

            if (OpenDate > DueDate)
            {
                result.code = AjaxResultCodeEnum.Error;
                result.message = "开始时间不能大于结束时间！";
                return Json(result);
            }

            if (string.IsNullOrEmpty(questions))
            {
                result.code = AjaxResultCodeEnum.Error;
                result.message = "试题不能为空！";
                return Json(result);
            }

            #endregion
            int subjectId = 0;
            Const.CourseSubjectMapping.TryGetValue(courseId, out subjectId);
            //保存作业信息
            T_Zy zy = new T_Zy()
            {
                UserId = UserId,
                ZyName = zyName,
                OpenDate = OpenDate,
                DueDate = DueDate,
                Type = 0,
                CourseId = courseId,
                SubjectId = subjectId,
                CreateDate = DateTime.Now,
                Ip = ClientUtil.Ip,
                IMEI = ClientUtil.IMEI,
                MobileBrand = ClientUtil.MobileBrand,
                SystemType = Request.Browser.Platform.ToString(),
                Browser = Request.Browser.Browser.ToString(),
                Status = 0
            };
            int id = B_Zy.Create(zy);

            //保存作业试题信息
            string[] qs = questions.Split(',');
            if (id > 0)
            {
                SaveZyQuestions(courseId, id, qs);
            }

            //修改试题使用次数
            IncreaseQuesUsageTimes(courseId, qs);

            result.code = AjaxResultCodeEnum.Success;
            result.message = "";
            return Json(result);
        }

        /// <summary>
        /// 保存作业试题
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="zyId"></param>
        /// <param name="questions"></param>
        /// <returns></returns>
        private bool SaveZyQuestions(int courseId, int zyId, string[] questions)
        {
            int OrderIndex = 0;
            List<dto_ZyQuestion> ql = new List<dto_ZyQuestion>();
            
            foreach (var qid in questions)
            {
                dto_ZyQuestion q = null;
                dto_Question dq = B_QuesRedis.GetQuestion(courseId, IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Ques, long.Parse(qid)));
                if (dq.haschildren && dq.Children != null)
                {
                    foreach (var cq in dq.Children)
                    {
                        q = new dto_ZyQuestion();
                        OrderIndex += 1;
                        q.PQId = dq.id;
                        q.QId = cq.id;
                        q.PTypeId = cq.ptypeid;
                        q.OrderIndex = OrderIndex;
                        q.Score = 0;
                        ql.Add(q);
                    }
                }
                else
                {
                    q = new dto_ZyQuestion();
                    OrderIndex += 1;
                    q.PQId = dq.id;
                    q.QId = dq.id;
                    q.PTypeId = dq.ptypeid;
                    q.OrderIndex = OrderIndex;
                    q.Score = 0;
                    ql.Add(q);
                }
            }
            return B_Zy.AddQdbZyQues(zyId, JsonConvert.SerializeObject(ql));
        }

        /// <summary>
        /// 试题使用次数加一
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="questions"></param>
        private void IncreaseQuesUsageTimes(int courseId, string[] questions)
        {
            
            foreach (var q in questions)
            {
                B_QuesRedis.IncreaseUsageTimes(IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Ques, long.Parse(q)));
            }

            B_Ques.IncreaseUsageTimes(courseId, questions);
        }
    }
}