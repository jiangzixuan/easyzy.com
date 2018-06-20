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
            return View(zy);
        }

        /// <summary>
        /// 提交统计
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="orderType">0：按照提交时间排序（默认）1：按照客观题正确数排序</param>
        /// <returns></returns>
        public ActionResult GetSubmitBar(long zyId, int orderType)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            List<dto_Answer> al = B_Answer.GetAnswers(id);

            int ObjectiveCount = JsonConvert.DeserializeObject<List<dto_ZyQuestion>>(B_ZyRedis.GetQdbZyQuesJson(id)).Count(a => Const.OBJECTIVE_QUES_TYPES.Contains(a.PTypeId));
            if (al != null)
            {
                al.ForEach(a =>
                {
                    if (a.StudentId == 0)
                    {
                        a.StudentName = "试用账号";
                    }
                    else
                    {
                        dto_User du = B_UserRedis.GetUser(a.StudentId);
                        a.StudentName = (du == null || string.IsNullOrEmpty(du.TrueName)) ? "未知姓名" : du.TrueName;
                    }

                    var ansl = JsonConvert.DeserializeObject<List<dto_UserAnswer>>(a.AnswerJson);
                    a.ObjectCorrectCount = (ansl.Count(ans => Const.OBJECTIVE_QUES_TYPES.Contains(ans.PTypeId) && ans.Answer == ans.CAnswer));

                });
                if (orderType == 0)
                {
                    al = al.OrderBy(a => a.CreateDate).ToList();
                }
                else
                {
                    al = al.OrderByDescending(a => a.ObjectCorrectCount).ToList();
                }

                ViewBag.xData = string.Join(",", al.Select(a => string.Concat(a.StudentName, "", a.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"))).ToArray());
                ViewBag.yData = string.Join(",", al.Select(a => a.ObjectCorrectCount).ToArray());
            }
            ViewBag.ObjectiveCount = ObjectiveCount;
            return PartialView();
        }

        /// <summary>
        /// 试题统计
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="orderType">0：按照题序排序 1：按照错误数排序</param>
        /// <returns></returns>
        public ActionResult GetQuesBar(long zyId, int orderType)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            
            //X轴题号
            List<string> ObjectiveQuesNum = new List<string>();
            List<dto_ZyQuestion> ql = JsonConvert.DeserializeObject<List<dto_ZyQuestion>>(B_ZyRedis.GetQdbZyQuesJson(id));
            int s = 0, t = 0, lastPQId = 0;
            if (ql != null)
            {
                foreach (var q in ql)
                {
                    if (Const.OBJECTIVE_QUES_TYPES.Contains(q.PTypeId))
                    {
                        if (q.PQId != lastPQId)
                        {
                            s += 1;
                            t = 1;
                            lastPQId = q.PQId;
                        }
                        else
                        {
                            t += 1;
                        }
                        if (q.PQId == q.QId)
                        {
                            ObjectiveQuesNum.Add(s.ToString());
                        }
                        else
                        {
                            ObjectiveQuesNum.Add(string.Concat(s, "-", t));
                        }
                    }
                }
            }
            //Y轴正确数
            int[] CorrectCount = new int[ObjectiveQuesNum.Count];
            List<dto_Answer> al = B_Answer.GetAnswers(id);
            int k = 0;
            if (al != null)
            {
                al.ForEach(a =>
                {
                    k = 0;
                    var ansl = JsonConvert.DeserializeObject<List<dto_UserAnswer>>(a.AnswerJson);
                    ansl.ForEach(b =>
                    {

                        if (Const.OBJECTIVE_QUES_TYPES.Contains(b.PTypeId))
                        {
                            if (b.Answer == b.CAnswer)
                            {
                                CorrectCount[k] = CorrectCount[k] + 1;
                            }
                            k += 1;
                        }
                    });

                });

             }
            //排序
            string[] xData = null;
            int[] yData = null;
            if (orderType == 0)
            {
                xData = ObjectiveQuesNum.ToArray();
                yData = CorrectCount;
            }
            else
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                for (int i = 0; i < ObjectiveQuesNum.Count; i++)
                {
                    d.Add(ObjectiveQuesNum[i], CorrectCount[i]);
                }
                xData = d.OrderBy(a => a.Value).Select(a => a.Key).ToArray();
                yData = d.OrderBy(a => a.Value).Select(a => a.Value).ToArray();
            }

            ViewBag.xData = string.Join(",", xData);
            ViewBag.yData = string.Join(",", yData);
            
            ViewBag.SubmitCount = al == null ? 0 : al.Count;
            return PartialView();
        }

        public ActionResult GetOptionBar(long zyId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);

            //题号
            List<string> ObjectiveQuesNum = new List<string>();
            List<dto_ZyQuestion> ql = JsonConvert.DeserializeObject<List<dto_ZyQuestion>>(B_ZyRedis.GetQdbZyQuesJson(id));
            int s = 0, t = 0, lastPQId = 0;
            if (ql != null)
            {
                foreach (var q in ql)
                {
                    if (Const.OBJECTIVE_QUES_TYPES.Contains(q.PTypeId))
                    {
                        if (q.PQId != lastPQId)
                        {
                            s += 1;
                            t = 1;
                            lastPQId = q.PQId;
                        }
                        else
                        {
                            t += 1;
                        }
                        if (q.PQId == q.QId)
                        {
                            ObjectiveQuesNum.Add(s.ToString());
                        }
                        else
                        {
                            ObjectiveQuesNum.Add(string.Concat(s, "-", t));
                        }
                    }
                }
            }

            List<dto_Answer> al = B_Answer.GetAnswers(id);
            int[] ACount = new int[ObjectiveQuesNum.Count];
            int[] BCount = new int[ObjectiveQuesNum.Count];
            int[] CCount = new int[ObjectiveQuesNum.Count];
            int[] DCount = new int[ObjectiveQuesNum.Count];
            int k = 0;
            if (al != null)
            {
                al.ForEach(a =>
                {
                    k = 0;
                    var ansl = JsonConvert.DeserializeObject<List<dto_UserAnswer>>(a.AnswerJson);
                    ansl.ForEach(b =>
                    {
                        if (Const.OBJECTIVE_QUES_TYPES.Contains(b.PTypeId))
                        {
                            //兼容多选题
                            if (b.Answer.Contains("A"))
                            {
                                ACount[k] = ACount[k] + 1; 
                            }
                            if (b.Answer.Contains("B"))
                            {
                                BCount[k] = BCount[k] + 1;
                            }
                            if (b.Answer.Contains("C"))
                            {
                                CCount[k] = CCount[k] + 1;
                            }
                            if (b.Answer.Contains("D"))
                            {
                                DCount[k] = DCount[k] + 1;
                            }
                            k += 1;
                        }
                    });
                });
            }
            ViewBag.Category = string.Join(",", ObjectiveQuesNum.ToArray());
            ViewBag.AData = string.Join(",", ACount);
            ViewBag.BData = string.Join(",", BCount);
            ViewBag.CData = string.Join(",", CCount);
            ViewBag.DData = string.Join(",", DCount);

            ViewBag.SubmitCount = al == null ? 0 : al.Count;
            return PartialView();
        }
    }
}