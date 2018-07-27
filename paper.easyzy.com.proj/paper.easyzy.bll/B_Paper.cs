using easyzy.sdk;
using paper.easyzy.dal;
using paper.easyzy.model.entity;
using paper.easyzy.model.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace paper.easyzy.bll
{
    public class B_Paper
    {
        public static List<dto_Paper> SearchPapers(int courseId, int gradeId, int typeId, int paperYear, int areaId, int pageIndex, int pageSize, out int totalCount)
        {
            List<dto_Paper> list = D_Paper.SearchPapers(courseId, gradeId, typeId, paperYear, areaId, pageIndex, pageSize, out totalCount);
            
            return list;
        }

        public static List<dto_Paper> SetPaperSubmited(List<dto_Paper> list, int studentId)
        {
            if (list != null && studentId != 0)
            {
                int[] Submited = B_Paper.GetSubmitedPapers(studentId, list.Select(a => a.PaperId).ToArray());
                if (Submited != null)
                {
                    foreach (var s in Submited)
                    {
                        list.FirstOrDefault(a => a.PaperId == s).Submited = true;
                    }
                }
            }
            
            return list;
        }

        public static bool IsPaperSubmited(long paperId, int studentId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Paper, paperId);
            return D_Paper.IsPaperSubmited(studentId, id);
        }

        public static List<dto_Paper> ResetPaperId(List<dto_Paper> list)
        {
            if (list != null)
            {
                foreach (var l in list)
                {
                    l.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Paper, l.PaperId);

                    string gName = "";
                    Const.Grades.TryGetValue(l.GradeId, out gName);
                    l.GradeName = gName;
                    l.PaperId = 0;
                }
            }
            return list;
        }

        public static int[] GetSubmitedPapers(int studentId, int[] paperId)
        {
            if (paperId.Length == 0) return null;
            return D_Paper.GetSubmitedPapers(studentId, paperId);

        }

        public static List<dto_Question> GetPaperQuestions(int courseId, long paperId)
        {
            List<dto_Question> list = new List<dto_Question>();
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Paper, paperId);

            int[] qids = GetPaperQuesIds(id);
            foreach (var q in qids)
            {
                dto_Question dq = D_QuesRedis.GetQuestion(courseId, q);
                if (dq != null)
                {
                    //dq.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, dq.id);
                    //dq.id = 0;
                    //if (dq.Children != null && dq.Children.Count > 0)
                    //{
                    //    dq.Children.ForEach(a => {
                    //        a.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, a.id);
                    //        a.id = 0;
                    //    });
                    //}
                    list.Add(dq);
                }
            }
            
            return list;
        }

        private static int[] GetPaperQuesIds(int paperId)
        {
            dto_Paper p = D_PaperRedis.GetPaper(paperId);
            if (p == null) return null;
            if (p.QIds == "") return null;

            string[] s = p.QIds.Split(',');

            int[] result = Array.ConvertAll(s, new Converter<string, int>(StringToInt));
            return result;
        }

        private static int StringToInt(string s)
        {
            return int.Parse(s);
        }

        public static dto_Paper GetPaper(long paperId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Paper, paperId);
            return D_PaperRedis.GetPaper(id);
        }

        public static T_Answer GetAnswer(long paperId, int studentId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Paper, paperId);
            return D_Paper.GetAnswer(id, studentId);
        }

        public static string SubmitAnswer(int courseId, long paperId, int studentId, string questions, string answers, string systemType, string browser)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Paper, paperId);

            //作业提交验证
            T_Answer ans = D_Paper.GetAnswer(id, studentId);
            if (ans != null && ans.Submited)
            {
               return "不能重复提交！";
            }

            //todo submit
            List<string> submitQlist = questions.Split(',').ToList();
            List<string> submitAlist = string.IsNullOrEmpty(answers) ? new List<string>() : answers.Split(',').ToList();
            if (submitQlist.Count != submitAlist.Count)
            {
                return "试题信息有误，提交失败！";
            }
            
            List<dto_Question> ql = GetPaperQuestions(courseId, paperId);
            List<dto_UserAnswer> al = new List<dto_UserAnswer>();
            ql.ForEach(a => {
                if (a.haschildren && a.Children != null)
                {
                    foreach (var c in a.Children)
                    {
                        if (Const.OBJECTIVE_QUES_TYPES.Contains(c.ptypeid))
                        {
                            int i = submitQlist.IndexOf(IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, c.id).ToString());
                            al.Add(new dto_UserAnswer() { QId = c.id, PTypeId = c.ptypeid, Score = 0, Answer = (i == -1 ? "" : submitAlist[i]), CAnswer = c.quesanswer, Point = 0 });
                        }
                        else
                        {
                            al.Add(new dto_UserAnswer() { QId = c.id, PTypeId = c.ptypeid, Score = 0, Answer = "", CAnswer = "", Point = 0 });
                        }
                    }
                }
                else
                {
                    if (Const.OBJECTIVE_QUES_TYPES.Contains(a.ptypeid))
                    {
                        int i = submitQlist.IndexOf(IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, a.id).ToString());
                        al.Add(new dto_UserAnswer() { QId = a.id, PTypeId = a.ptypeid, Score = 0, Answer = (i == -1 ? "" : submitAlist[i]), CAnswer = a.quesanswer, Point = 0 });
                    }
                    else
                    {
                        al.Add(new dto_UserAnswer() { QId = a.id, PTypeId = a.ptypeid, Score = 0, Answer = "", CAnswer = "", Point = 0 });
                    }
                }
            });
            bool isok = false;

            T_Answer answer = new T_Answer()
            {
                PaperId = id,
                StudentId = studentId,
                Submited = true,
                CreateDate = DateTime.Now,
                AnswerJson = JsonConvert.SerializeObject(al),
                AnswerImg = "",
                Ip = ClientUtil.Ip,
                IMEI = ClientUtil.IMEI,
                MobileBrand = ClientUtil.MobileBrand,
                SystemType = systemType,
                Browser = browser
            };

            if (ans != null)
            {
                isok = D_Paper.UpdateAnswerJson(id, studentId, answer.AnswerJson);
            }
            else
            {
                isok = D_Paper.InsertZyAnswer(answer);
            }
            return isok ? "" : "入库失败！";
        }

        public static List<dto_Question> GetPaperAnswer(int courseId, long paperId, int studentId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Paper, paperId);
            
            var answer = D_Paper.GetAnswer(id, studentId);
            List<dto_UserAnswer> ansl = null;
            if (answer != null)
            {
                ansl = JsonConvert.DeserializeObject<List<dto_UserAnswer>>(answer.AnswerJson);
            }
            List<dto_Question> ql = B_Paper.GetPaperQuestions(courseId, paperId);
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
            return ql;
        }

        #region 修改T_Paper的QuestionIds字段，从OriginalQuesId修改为QuesId
        public static void ModifyPaperQuestions(int courseId)
        {
            int totalCount = 0;
            Stopwatch sw = new Stopwatch();
            List<dto_Paper> papers = D_Paper.SearchPapers(courseId, 0, 0, 0, 0, 1, 20000, out totalCount);
            LogHelper.Error("---------------CourseId：" + courseId + "，转化T_Paper表的QuestionIds开始，总数：" + totalCount + "个----------------");
            sw.Start();
            if (papers != null)
            {
                for (int i = 0; i < papers.Count; i++)
                {
                    if (i % 100 == 0) { LogHelper.Error("---------------第" + (i + 1) + "题开始----------------"); }
                    
                    if (!string.IsNullOrEmpty(papers[i].QuestionIds))
                    {
                        string[] qids = JsonConvert.DeserializeObject<string[]>(papers[i].QuestionIds);
                        int[] l = D_Ques.GetQuestionsBySourceId(papers[i].CourseId, string.Join(",", qids));
                        D_Paper.UpdatePaperQIds(papers[i].PaperId, string.Join(",", l));
                    }
                }
            }
            sw.Stop();
            LogHelper.Error("---------------CourseId：" + courseId + "，转化结束，用时：" + sw.Elapsed + "秒----------------");
            
        }

        #endregion
    }
}
