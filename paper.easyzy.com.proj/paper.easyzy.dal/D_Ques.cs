using easyzy.sdk;
using paper.easyzy.model.dto;
using paper.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace paper.easyzy.dal
{
    public class D_Ques
    {
        private static string QuesConnString = "";
        static D_Ques()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Ques, out QuesConnString);
        }

        public static dto_Question GetWholeQuestion(int courseId, int qId)
        {
            dto_Question dq = GetQuestion(courseId, qId);
            if (dq != null)
            {
                if (dq.haschildren)
                {
                    List<dto_CQuestion> cdq = null;
                    List<dto_CQuestion> cq = GetChildQuestions(dq.id);

                    if (cq != null)
                    {
                        cdq = new List<dto_CQuestion>();
                        List<T_QuesOptions> qo = null;
                        if (cq.Exists(a => Const.OBJECTIVE_QUES_TYPES.Contains(a.ptypeid)))
                        {
                            qo = GetAllChildQuesOptions(dq.id);
                        }
                        cq.ForEach(a =>
                        {
                            if (Const.OBJECTIVE_QUES_TYPES.Contains(a.ptypeid))
                            {
                                a.Options = qo.Find(b => b.id == a.id);
                            }
                            cdq.Add(a);
                        });
                    }
                    dq.Children = cdq;
                }
                else
                {
                    if (Const.OBJECTIVE_QUES_TYPES.Contains(dq.ptypeid))
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
        public static dto_Question GetQuestion(int courseId, int qId)
        {
            dto_Question model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select id, courseid, ptypeid, typeid, typename, difftype, diff, haschildren, quesbody, quesanswer, quesparse, pid, usagetimes from T_Questions where courseid = @courseid and id = @id",
                "@courseid".ToInt32InPara(courseId), "@id".ToInt32InPara(qId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<dto_Question>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 根据大题Id获取其所有小题信息（不包含选项）
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static List<dto_CQuestion> GetChildQuestions(int pId)
        {
            List<dto_CQuestion> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select id, pid, ptypeid, typeid, typename, quesbody, quesanswer, quesparse from T_CQuestions where pid = @pid order by id",
                "@pid".ToInt32InPara(pId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_CQuestion>(dr);
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
                "select id, optiona, optionb, optionc, optiond, optione, optionf, optiong, pid from T_QuesOptions where pid = @pid",
                "@pid".ToInt32InPara(pId)))
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
                "select id, optiona, optionb, optionc, optiond, optione, optionf, optiong from T_QuesOptions where id = @id",
                "@id".ToInt32InPara(qId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_QuesOptions>(dr);
                }
            }
            return model;
        }

    }
}
