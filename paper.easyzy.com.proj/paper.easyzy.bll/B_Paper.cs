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
                    list.Add(dq);
                }
            }
            
            return list;
        }

        private static int[] GetPaperQuesIds(int paperId)
        {
            dto_Paper p = D_Paper.GetPaper(paperId);
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
