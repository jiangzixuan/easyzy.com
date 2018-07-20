using easyzy.sdk;
using MySql.Data.MySqlClient;
using paper.easyzy.model.entity;
using paper.easyzy.model.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paper.easyzy.dal
{
    public class D_Paper
    {
        private static string QuesConnString = "";
        static D_Paper()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Ques, out QuesConnString);
        }

        /// <summary>
        /// 搜索试卷
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="gradeId"></param>
        /// <param name="typeId"></param>
        /// <param name="paperYear"></param>
        /// <param name="areaId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<dto_Paper> SearchPapers(int courseId, int gradeId, int typeId, int paperYear, int areaId, int pageIndex, int pageSize, out int totalCount)
        {
            List<dto_Paper> list = null;
            string whereStr = " T_Paper where CourseId=@CourseId ";
            List<MySqlParameter> param = new List<MySqlParameter>();
            param.Add(new MySqlParameter("@CourseId", MySqlDbType.Int32) { Value = courseId });
            if (gradeId != 0)
            {
                whereStr = whereStr + " and GradeId=@GradeId";
                param.Add(new MySqlParameter("@GradeId", MySqlDbType.Int32) { Value = gradeId });
            }

            if (typeId != 0)
            {
                whereStr = whereStr + " and TypeId=@TypeId";
                param.Add(new MySqlParameter("@TypeId", MySqlDbType.Int32) { Value = typeId });
            }

            if (paperYear != 0 && paperYear > 0)
            {
                whereStr = whereStr + " and PaperYear=@PaperYear";
                param.Add(new MySqlParameter("@PaperYear", MySqlDbType.Int32) { Value = paperYear });
            }
            else if (paperYear < 0)
            {
                whereStr = whereStr + " and PaperYear<=" + paperYear * -1;
                param.Add(new MySqlParameter("@PaperYear", MySqlDbType.Int32) { Value = paperYear });
            }

            if (areaId != 0)
            {
                whereStr = whereStr + " and AreaId=@AreaId";
                param.Add(new MySqlParameter("@AreaId", MySqlDbType.Int32) { Value = areaId });
            }

            using (MySqlDataReader dr = MySqlDBHelper.GetPageReader(Util.GetConnectString(QuesConnString),
                "PaperId, CourseId, TypeId, GradeId, AreaId, PaperYear, Title, QuestionIds",
                whereStr,
                "PaperYear desc",
                pageSize,
                pageIndex,
                out totalCount,
                param.ToArray()
                ))
            {
                if (dr != null && dr.HasRows)
                {
                    list = MySqlDBHelper.ConvertDataReaderToEntityList<dto_Paper>(dr);
                }
            }
            return list;
        }

        public static dto_Paper GetPaper(int paperId)
        {
            dto_Paper p = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select PaperId, CourseId, TypeId, GradeId, AreaId, PaperYear, Title, QIds from T_Paper where PaperId = @PaperId",
                "@PaperId".ToInt32InPara(paperId)))
            {
                if (dr != null && dr.HasRows)
                {
                    p = MySqlDBHelper.ConvertDataReaderToEntitySingle<dto_Paper>(dr);
                }
            }
            return p;
        }

        public static void UpdatePaperQIds(int paperId, string qids)
        {
            MySqlHelper.ExecuteNonQuery(Util.GetConnectString(QuesConnString),
                "update T_Paper set QIds = @QIds where PaperId = @PaperId",
                "@QIds".ToVarCharInPara(qids),
                "@PaperId".ToInt32InPara(paperId));
        }
    }
}
