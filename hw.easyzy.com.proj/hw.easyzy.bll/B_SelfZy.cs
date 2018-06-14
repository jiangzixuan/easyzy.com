using easyzy.sdk;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.bll
{
    /// <summary>
    /// 旧的自传作业使用
    /// </summary>
    public class B_SelfZy
    {
        private static string ZyConnString = "";
        static B_SelfZy()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Zy, out ZyConnString);
        }
        /// <summary>
        /// 新增作业结构
        /// </summary>
        /// <param name="zysl"></param>
        /// <returns></returns>
        public static int AddZyStruct(List<T_ZyStruct> zysl)
        {
            string sql = "";
            StringBuilder sb = new StringBuilder();
            foreach (var item in zysl)
            {
                sb.AppendFormat(",(null, {0},{1},{2},{3},'{4}','{5}')", item.ZyId, item.BqNum, item.SqNum, item.QuesType, item.QuesAnswer, item.CreateDate);
            }
            sql = sb.ToString().Substring(1);
            sql = "insert into T_ZyStruct(Id, ZyId, BqNum, SqNum, QuesType, QuesAnswer, CreateDate) values" + sql;
            return MySqlHelper.ExecuteNonQuery(Util.GetConnectString(ZyConnString), sql);
        }

        /// <summary>
        /// 修改作业状态为设置过答题卡
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public static int UpdateZyStructed(int zyId)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(ZyConnString),
                "update T_Zy set Structed = 1 where Id = @Id",
                "@Id".ToInt32InPara(zyId)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        /// <summary>
        /// 获取作业结构
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public static List<T_ZyStruct> GetZyStruct(int zyId)
        {
            List<T_ZyStruct> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select Id, ZyId, BqNum, SqNum, QuesType, QuesAnswer, CreateDate from T_ZyStruct where ZyId = @ZyId",
                "@ZyId".ToInt32InPara(zyId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_ZyStruct>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 提交答案
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        

        /// <summary>
        /// 根据真实姓名判断作业是否提交过
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="trueName"></param>
        /// <returns></returns>
        public static bool IsZySubmited(int zyId, string trueName)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(ZyConnString),
                "select 1 from T_Answer where ZyId = @ZyId and TrueName = @TrueName limit 1",
                "@ZyId".ToInt32InPara(zyId),
                "@TrueName".ToVarCharInPara(trueName));
            return o == null ? false : true;
        }

        /// <summary>
        /// 获取学生提交的作业答案
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public static List<T_Answer> GetZyAnswers(int zyId)
        {
            List<T_Answer> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select Id, ZyId, StudentId, TrueName, AnswerJson, AnswerImg, CreateDate from T_Answer where ZyId = @ZyId order by CreateDate",
                "@ZyId".ToInt32InPara(zyId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_Answer>(dr);
                }
            }
            return model;
        }

        public static T_Answer GetZyAnswer(int zyId, string trueName)
        {
            T_Answer model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select Id, ZyId, StudentId, TrueName, AnswerJson, AnswerImg, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser from T_Answer where ZyId = @ZyId and TrueName = @TrueName",
                "@ZyId".ToInt32InPara(zyId),
                "@TrueName".ToVarCharInPara(trueName)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_Answer>(dr);
                }
            }
            return model;
        }
        
    }
}
