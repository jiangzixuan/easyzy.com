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
        private static string PaperConnString = "";
        static D_Paper()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Ques, out QuesConnString);
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Paper, out PaperConnString);
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

        /// <summary>
        /// 提交答案
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool InsertZyAnswer(T_Answer a)
        {
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(PaperConnString),
                "insert into T_Answer(PaperId, StudentId, AnswerJson, AnswerImg, Submited, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser) values (@ZyId, @StudentId, @AnswerJson, @AnswerImg, @Submited, @CreateDate, @Ip, @IMEI, @MobileBrand, @SystemType, @Browser);",
                "@ZyId".ToInt32InPara(a.PaperId),
                "@StudentId".ToInt32InPara(a.StudentId),
                "@AnswerJson".ToVarCharInPara(a.AnswerJson),
                "@AnswerImg".ToVarCharInPara(a.AnswerImg),
                "@Submited".ToBitInPara(a.Submited),
                "@CreateDate".ToDateTimeInPara(a.CreateDate),
                "@Ip".ToVarCharInPara(a.Ip),
                "@IMEI".ToVarCharInPara(a.IMEI),
                "@MobileBrand".ToVarCharInPara(a.MobileBrand),
                "@SystemType".ToVarCharInPara(a.SystemType),
                "@Browser".ToVarCharInPara(a.Browser)
                );
            return i > 0;
        }

        /// <summary>
        /// 修改作业的AnswerJson并提交作业
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="userId"></param>
        /// <param name="answerJson"></param>
        /// <returns></returns>
        public static bool UpdateAnswerJson(int paperId, int userId, string answerJson)
        {
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(PaperConnString),
                "update T_Answer set CreateDate = now(), AnswerJson = @AnswerJson, Submited = 1 where PaperId = @PaperId and StudentId = @StudentId",
                "@PaperId".ToInt32InPara(paperId),
                "@StudentId".ToInt32InPara(userId),
                "@AnswerJson".ToVarCharInPara(answerJson)
                );
            return i > 0;
        }

        /// <summary>
        /// 获取答案
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static T_Answer GetAnswer(int paperId, int userId)
        {
            T_Answer model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(PaperConnString),
                "select Id, PaperId, StudentId, AnswerJson, AnswerImg, Submited, CreateDate from T_Answer where PaperId = @PaperId and StudentId = @StudentId",
                "@PaperId".ToInt32InPara(paperId),
                "@StudentId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_Answer>(dr);
                }
            }
            return model;
        }

        public static int[] GetSubmitedPapers(int studentId, int[] paperId)
        {
            List<int> result = null;
            string ps = string.Join(",", paperId);
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(PaperConnString),
                "select PaperId from T_Answer where PaperId in (" + ps + ") and StudentId = @StudentId and Submited = 1",
                "@StudentId".ToInt32InPara(studentId)))
            {
                if (dr != null && dr.HasRows)
                {
                    result = new List<int>();
                    while (dr.Read())
                    {
                        result.Add(int.Parse(dr[0].ToString()));
                    }
                }
            }
            return result == null ? null : result.ToArray();

        }

        public static bool IsPaperSubmited(int studentId, int paperId)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(PaperConnString),
                "select 1 from T_Answer where PaperId = @PaperId and StudentId = @StudentId and Submited = 1",
                "@StudentId".ToInt32InPara(studentId),
                "@PaperId".ToInt32InPara(paperId));

            return o == null ? false : true;

        }
    }
}
