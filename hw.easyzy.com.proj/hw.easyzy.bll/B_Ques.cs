using easyzy.sdk;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace hw.easyzy.bll
{
    public class B_Ques
    {
        private static string QuesConnString = "";
        static B_Ques()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Ques, out QuesConnString);
        }

        /// <summary>
        /// 根据查询条件返回符合条件的试题Id数组
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="kpointId"></param>
        /// <param name="cpointId"></param>
        /// <param name="typeId"></param>
        /// <param name="diffType"></param>
        /// <param name="paperYear"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static int[] GetQuesIds(int courseId, int kpointId, int cpointId, int typeId, int diffType, int paperYear, int pageIndex, int pageSize, out int totalCount)
        {
            List<int> model = null;
            string wherestr = "T_Questions where courseid = @courseId and pid = 0 ";
            string orderstr = "usagetimes desc";
            List<MySqlParameter> pl = new List<MySqlParameter>();
            pl.Add(new MySqlParameter("@courseid", courseId));

            if (typeId != 0)
            {
                wherestr += "and typeid=@typeid ";
                pl.Add(new MySqlParameter("@typeid", typeId));
            }
            if (diffType != 0)
            {
                wherestr += "and difftype=@difftype ";
                pl.Add(new MySqlParameter("@difftype", diffType));
            }

            if (paperYear != 0 && paperYear > 0)
            {
                wherestr += "and paperyear = @paperyear";
                pl.Add(new MySqlParameter("@paperyear", paperYear));
            }
            else if (paperYear < 0)
            {
                wherestr += "and paperyear <= @paperyear";
                pl.Add(new MySqlParameter("@paperyear", paperYear * -1));
            }

            if (kpointId != 0)
            {
                wherestr += "and kpoints like '%\"" + kpointId + "\"%'";
            }

            if (cpointId != 0)
            {
                int[] s = B_QuesBase.GetSimilarCatalogs(courseId, cpointId);
                if (s != null)
                {
                    wherestr += "and (";
                    var str = "";
                    foreach (var c in s)
                    {
                        str += " or cpoints like '%\"" + c + "\"%'";
                    }
                    wherestr += str.Substring(3) + ")";
                }
                else
                {
                    wherestr += "and cpoints like '%\"" + cpointId + "\"%'";
                }
            }

            using (MySqlDataReader dr = MySqlDBHelper.GetPageReader(Util.GetConnectString(QuesConnString),
                "id",
                wherestr,
                orderstr,
                pageSize,
                pageIndex,
                out totalCount,
                pl.ToArray()))
            {
                if (dr != null && dr.HasRows)
                {
                    while (dr.Read())
                    {
                        model.Add(int.Parse(dr[0].ToString()));
                    }
                }
            }
            return model == null ? null : model.ToArray();
            
        }


        public static dto_Question GetWholeQuestion(int qId)
        {
            dto_Question dq = null;
            T_Questions q = GetQuestion(qId);
            if (q != null)
            {
                dq = (dto_Question)q;
                if (dq.haschildren)
                {
                    List<dto_CQuestion> cdq = null;
                    List<T_CQuestions> cq = GetChildQuestions(dq.id);

                    if (cq != null)
                    {
                        cdq = new List<dto_CQuestion>();
                        List<T_QuesOptions> qo = null;
                        if (cq.Exists(a => Const.OBJECTIVE_QUES_TYPES.Contains(a.typeid)))
                        {
                            qo = GetAllChildQuesOptions(dq.id);
                        }
                        cq.ForEach(a =>
                        {
                            dto_CQuestion c = (dto_CQuestion)a;
                            if (Const.OBJECTIVE_QUES_TYPES.Contains(c.typeid))
                            {
                                c.Options = qo.Find(b => b.qid == a.id);
                            }
                            cdq.Add(c);
                        });
                    }
                    dq.Children = cdq;
                }
                else
                {
                    if (Const.OBJECTIVE_QUES_TYPES.Contains(dq.typeid))
                    {
                        dq.Options = GetQuesOption(dq.id);
                    }
                }
            }
            return dq;
        }

        /// <summary>
        /// 根据试题Id获取其试题信息（不包含任何小题、选项）
        /// </summary>
        /// <param name="qId"></param>
        /// <returns></returns>
        public static T_Questions GetQuestion(int qId)
        {
            T_Questions model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select id, courseid, typeid, typename, difftype, diff, haschildren, quesbody, quesanswer, quesparse, pid from T_Questions where Id = @Id",
                "@Id".ToInt32InPara(qId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_Questions>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 根据大题Id获取其所有小题信息（不包含选项）
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static List<T_CQuestions> GetChildQuestions(int pId)
        {
            List<T_CQuestions> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select id, pid, orderindex, typeid, typename, quesbody, quesanswer, quesparse from T_CQuestions where pid = @pid",
                "@pid".ToInt32InPara(pId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_CQuestions>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 根据大题Id一次获取所有小题的选项
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static List<T_QuesOptions> GetAllChildQuesOptions(int pId)
        {
            List<T_QuesOptions> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select qid, optiona, optionb, optionc, optiond, optione, optionf, optiong from T_QuesOptions where pid = @pid order by orderindex",
                "@qid".ToInt32InPara(pId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_QuesOptions>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 根据试题Id获取其选项信息
        /// </summary>
        /// <param name="qId"></param>
        /// <returns></returns>
        public static T_QuesOptions GetQuesOption(int qId)
        {
            T_QuesOptions model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select qid, optiona, optionb, optionc, optiond, optione, optionf, optiong from T_QuesOptions where qid = @qid",
                "@qid".ToInt32InPara(qId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_QuesOptions>(dr);
                }
            }
            return model;
        }

        #region 知识点/教材目录

        public static List<T_CatalogNodes> GetCatalogNodes(int textbookId)
        {
            List<T_CatalogNodes> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select Id, Name, Ordinal, ParentId, TextBookId, Type from T_CatalogNodes a where a.TextBookId = @TextBookId order by ParentId, Ordinal",
                "@TextBookId".ToInt32InPara(textbookId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_CatalogNodes>(dr);
                }
            }
            return model;
        }

        public static List<T_KnowledgePoints> GetKnowledgePoints(int courseId)
        {
            List<T_KnowledgePoints> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select Id, CourseId, Name, Ordinal, ParentId, Type from T_KnowledgePoints a where a.CourseId = @CourseId order by ParentId, Ordinal",
                "@CourseId".ToInt32InPara(courseId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_KnowledgePoints>(dr);
                }
            }
            return model;
        }
        
        #endregion

    }
}
