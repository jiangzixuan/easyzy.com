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

        public static List<dto_Question> GetPaperQuestions(long paperId)
        {
            List<dto_Question> list = null;
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Paper, paperId);
            return list;
        }

        private static int[] GetPaperQuesIds(int paperId)
        {
            dto_Paper p = D_Paper.GetPaper(paperId);
            if (p == null) return null;
            if (p.QuestionIds == "") return null;

            string[] s = JsonConvert.DeserializeObject<string[]>(p.QuestionIds);

            int[] result = Array.ConvertAll(s, new Converter<string, int>(StringToInt));
            return result;
        }

        private static int StringToInt(string s)
        {
            return int.Parse(s);
        }
    }
}
