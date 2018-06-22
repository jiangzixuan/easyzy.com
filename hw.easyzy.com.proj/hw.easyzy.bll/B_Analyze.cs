using easyzy.sdk;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.bll
{
    /// <summary>
    /// 提交之后异步计算Analyze数据
    /// 应该调用analyze.easyzy.com/api/来实现
    /// 暂时写在这里
    /// </summary>
    public class B_Analyze
    {
        private static string AnalyzeConnString = "";
        static B_Analyze()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Analyze, out AnalyzeConnString);
        }

        /// <summary>
        /// 获取提交过某作业的班级列表
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public static List<dto_ClassSubmitCount> GetSubmitClasses(int zyId)
        {
            List<dto_ClassSubmitCount> list = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(AnalyzeConnString),
                "select SchoolId, GradeId, ClassId, SubmitCount from T_ClassSubmitCount where ZyId = @ZyId and SubmitCount > 0",
                "@ZyId".ToInt32InPara(zyId)))
            {
                if (dr != null && dr.HasRows)
                {
                    list = MySqlDBHelper.ConvertDataReaderToEntityList<dto_ClassSubmitCount>(dr);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取作业提交数
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="schoolId"></param>
        /// <param name="gradeId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static int GetZySubmitCount(int zyId, int schoolId, int gradeId, int classId)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(AnalyzeConnString),
                "select SubmitCount from T_ClassSubmitCount where ZyId = @ZyId and SchoolId = @SchoolId and GradeId = @GradeId and ClassId = @ClassId limit 1",
                "@ZyId".ToInt32InPara(zyId),
                "@SchoolId".ToInt32InPara(schoolId),
                "@GradeId".ToInt32InPara(gradeId),
                "@ClassId".ToInt32InPara(classId));
            return o == null ? 0 : int.Parse(o.ToString());
        }

        /// <summary>
        /// 学生提交及得分报表
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="schoolId"></param>
        /// <param name="gradeId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static dto_Echart_Bar GetStudentPoint(int zyId, int schoolId, int gradeId, int classId)
        {
            dto_Echart_Bar deb = null;
            List<string> x = null;
            List<string> y = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(AnalyzeConnString),
                "select StudentId, SubmitDate, Score from T_StudentPoint where ZyId = @ZyId and SchoolId = @SchoolId and GradeId = @GradeId and ClassId = @ClassId order by SubmitDate",
                "@ZyId".ToInt32InPara(zyId),
                "@SchoolId".ToInt32InPara(schoolId),
                "@GradeId".ToInt32InPara(gradeId),
                "@ClassId".ToInt32InPara(classId)))
            {
                if (dr != null && dr.HasRows)
                {
                    deb = new dto_Echart_Bar();
                    x = new List<string>();
                    y = new List<string>();
                    while (dr.Read())
                    {
                        x.Add(string.Concat(dr[0].ToString()));
                        y.Add(dr[2].ToString());
                    }
                    deb.x = x;
                    deb.y = y;
                }
            }
            
            return deb;
        }

        /// <summary>
        /// 作业试题正确数报表
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="schoolId"></param>
        /// <param name="gradeId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static dto_Echart_Bar GetQuesCorrectCount(int zyId, int schoolId, int gradeId, int classId)
        {
            dto_Echart_Bar deb = null;
            List<string> x = null;
            List<string> y = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(AnalyzeConnString),
                "select QuesNumTip, Count from T_QuesCorrectCount where ZyId = @ZyId and SchoolId = @SchoolId and GradeId = @GradeId and ClassId = @ClassId order by QuesNum",
                "@ZyId".ToInt32InPara(zyId),
                "@SchoolId".ToInt32InPara(schoolId),
                "@GradeId".ToInt32InPara(gradeId),
                "@ClassId".ToInt32InPara(classId)))
            {
                if (dr != null && dr.HasRows)
                {
                    deb = new dto_Echart_Bar();
                    x = new List<string>();
                    y = new List<string>();
                    while (dr.Read())
                    {
                        x.Add(string.Concat("第", dr[0].ToString(), "题"));
                        y.Add(dr[1].ToString());
                    }
                    deb.x = x;
                    deb.y = y;
                }
            }

            return deb;
        }

        public static dto_Echart_Bar2 GetOptionSelectCount(int zyId, int schoolId, int gradeId, int classId)
        {
            dto_Echart_Bar2 deb = null;
            List<string> category = null;
            List<string> optiona = null;
            List<string> optionb = null;
            List<string> optionc = null;
            List<string> optiond = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(AnalyzeConnString),
                "select QuesNumTip, OptionA, OptionB, OptionC, OptionD from T_OptionSelectCount where ZyId = @ZyId and SchoolId = @SchoolId and GradeId = @GradeId and ClassId = @ClassId order by QuesNum",
                "@ZyId".ToInt32InPara(zyId),
                "@SchoolId".ToInt32InPara(schoolId),
                "@GradeId".ToInt32InPara(gradeId),
                "@ClassId".ToInt32InPara(classId)))
            {
                if (dr != null && dr.HasRows)
                {
                    deb = new dto_Echart_Bar2();
                    category = new List<string>();
                    optiona = new List<string>();
                    optionb = new List<string>();
                    optionc = new List<string>();
                    optiond = new List<string>();
                    while (dr.Read())
                    {
                        category.Add(string.Concat("第", dr[0].ToString(), "题"));
                        optiona.Add(dr[1].ToString());
                        optionb.Add(dr[2].ToString());
                        optionc.Add(dr[3].ToString());
                        optiond.Add(dr[4].ToString());
                    }
                    deb.category = category;
                    deb.optiona = optiona;
                    deb.optionb = optionb;
                    deb.optionc = optionc;
                    deb.optiond = optiond;
                }
            }

            return deb;
        }

        /// <summary>
        /// 生成统计信息
        /// </summary>
        /// <param name="answer"></param>
        public static void GenerateAnalyze(T_Answer answer)
        {
            Task.Run(() =>
            {
                dto_User u = null;
                if (answer.StudentId == 0)
                {
                    u = new dto_User() { SchoolId = 0, GradeId = 0, ClassId = 0, TrueName = "试用用户" };
                }
                else
                {
                    u = B_UserRedis.GetUser(answer.StudentId);
                }

                IncreaseClassSubmitCount(answer.ZyId, u);

                var ansl = JsonConvert.DeserializeObject<List<dto_UserAnswer>>(answer.AnswerJson);
                InsertStudentPoint(answer.ZyId, answer.StudentId, answer.CreateDate, ansl, u);
                IncreaseQuesCorrectCount(answer.ZyId, ansl, u);
                IncreaseOptionSelectCount(answer.ZyId, ansl, u);
            });
        }

        #region 写入统计表的私有方法

        /// <summary>
        /// 作业班级提交数量
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="u"></param>
        private static void IncreaseClassSubmitCount(int zyId, dto_User u)
        {
            MySqlHelper.ExecuteNonQuery(Util.GetConnectString(AnalyzeConnString),
                "insert into T_ClassSubmitCount(ZyId, SchoolId, GradeId, ClassId, SubmitCount) values (@ZyId, @SchoolId, @GradeId, @ClassId, 1) on duplicate key update SubmitCount = SubmitCount + 1",
                "@ZyId".ToInt32InPara(zyId),
                "@SchoolId".ToInt32InPara(u.SchoolId),
                "@GradeId".ToInt32InPara(u.GradeId),
                "@ClassId".ToInt32InPara(u.ClassId)
                );
        }

        /// <summary>
        /// 学生作业得分
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="studentId"></param>
        /// <param name="submitDate"></param>
        /// <param name="ansl"></param>
        /// <param name="u"></param>
        private static void InsertStudentPoint(int zyId, int studentId, DateTime submitDate, List<dto_UserAnswer> ansl, dto_User u)
        {
            int Score = (ansl.Count(ans => Const.OBJECTIVE_QUES_TYPES.Contains(ans.PTypeId) && ans.Answer == ans.CAnswer));

            MySqlHelper.ExecuteNonQuery(Util.GetConnectString(AnalyzeConnString),
                "insert into T_StudentPoint(ZyId, StudentId, SchoolId, GradeId, ClassId, SubmitDate, Score) values (@ZyId, @StudentId, @SchoolId, @GradeId, @ClassId, @SubmitDate, @Score)",
                "@ZyId".ToInt32InPara(zyId),
                "@StudentId".ToInt32InPara(studentId),
                "@SchoolId".ToInt32InPara(u.SchoolId),
                "@GradeId".ToInt32InPara(u.GradeId),
                "@ClassId".ToInt32InPara(u.ClassId),
                "@SubmitDate".ToDateTimeInPara(submitDate),
                "@Score".ToInt32InPara(Score)
                );
        }

        /// <summary>
        /// 试题正确数量
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="ansl"></param>
        /// <param name="u"></param>
        private static void IncreaseQuesCorrectCount(int zyId, List<dto_UserAnswer> ansl, dto_User u)
        {
            List<string> ObjectiveQuesNum = new List<string>();
            List<dto_ZyQuestion> ql = JsonConvert.DeserializeObject<List<dto_ZyQuestion>>(B_ZyRedis.GetQdbZyQuesJson(zyId));
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

            string sql = "insert into T_QuesCorrectCount(ZyId, QuesNum, QuesNumTip, SchoolId, GradeId, ClassId, Count) values ";

            for (int i = 0; i < ObjectiveQuesNum.Count; i++)
            {
                var oansl = ansl.FindAll(a => Const.OBJECTIVE_QUES_TYPES.Contains(a.PTypeId));
                sql += string.Format("({0}, {1}, '{2}', {3}, {4}, {5}, {6}),", zyId, i + 1, ObjectiveQuesNum[i], u.SchoolId, u.GradeId, u.ClassId, (oansl[i].Answer == oansl[i].CAnswer ? 1 : 0));
            }
            sql = sql.Substring(0, sql.Length - 1);
            sql += " on duplicate key update Count = Count + VALUES(Count)";
            MySqlHelper.ExecuteNonQuery(Util.GetConnectString(AnalyzeConnString), sql);
            
        }

        /// <summary>
        /// 试题选项选中数
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="ansl"></param>
        /// <param name="u"></param>
        private static void IncreaseOptionSelectCount(int zyId, List<dto_UserAnswer> ansl, dto_User u)
        {
            List<string> ObjectiveQuesNum = new List<string>();
            List<dto_ZyQuestion> ql = JsonConvert.DeserializeObject<List<dto_ZyQuestion>>(B_ZyRedis.GetQdbZyQuesJson(zyId));
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

            string sql = "insert into T_OptionSelectCount(ZyId, QuesNum, QuesNumTip, SchoolId, GradeId, ClassId, OptionA, OptionB, OptionC, OptionD) values ";
            
            for (int i = 0; i < ObjectiveQuesNum.Count; i++)
            {
                var oansl = ansl.FindAll(a => Const.OBJECTIVE_QUES_TYPES.Contains(a.PTypeId));

                sql += string.Format("({0}, {1}, '{2}', {3}, {4}, {5}, {6}, {7}, {8}, {9}),", zyId, i + 1, ObjectiveQuesNum[i], u.SchoolId, u.GradeId, u.ClassId, (oansl[i].Answer.Contains("A") ? 1 : 0), (oansl[i].Answer.Contains("B") ? 1 : 0), (oansl[i].Answer.Contains("C") ? 1 : 0), (oansl[i].Answer.Contains("D") ? 1 : 0));
            }
            sql = sql.Substring(0, sql.Length - 1);
            sql += " on duplicate key update OptionA = OptionA + VALUES(OptionA), OptionB = OptionB + VALUES(OptionB), OptionC = OptionC + VALUES(OptionC), OptionD = OptionD + VALUES(OptionD)";
            MySqlHelper.ExecuteNonQuery(Util.GetConnectString(AnalyzeConnString), sql);
        }

        #endregion


    }
}
