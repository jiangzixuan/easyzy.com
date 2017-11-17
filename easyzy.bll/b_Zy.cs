using easyzy.common;
using easyzy.model.entity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;

namespace easyzy.bll
{
    public class B_Zy
    {
        /// <summary>
        /// 查询作业
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static T_Zy GetZy(int Id)
        {
            T_Zy model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString("EasyZy_Home"),
                "select Id, UserId, BodyWordPath, BodyHtmlPath, AnswerWordPath, AnswerHtmlPath, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser, Structed from T_Zy where Id = @Id",
                "@Id".ToInt32InPara(Id)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_Zy>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 新建作业
        /// </summary>
        /// <param name="zy"></param>
        /// <returns></returns>
        public static int Create(T_Zy zy)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString("EasyZy_Home"),
                "insert into T_Zy(Id, UserId, BodyWordPath, BodyHtmlPath, AnswerWordPath, AnswerHtmlPath, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser, Structed) values (null, @UserId, @BodyWordPath, @BodyHtmlPath, @AnswerWordPath, @AnswerHtmlPath, @CreateDate, @Ip, @IMEI, @MobileBrand, @SystemType, @Browser, @Structed); select last_insert_id();",
                "@UserId".ToInt32InPara(zy.UserId),
                "@BodyWordPath".ToVarCharInPara(zy.BodyWordPath),
                "@BodyHtmlPath".ToVarCharInPara(zy.BodyHtmlPath),
                "@AnswerWordPath".ToVarCharInPara(zy.AnswerWordPath),
                "@AnswerHtmlPath".ToVarCharInPara(zy.AnswerHtmlPath),
                "@CreateDate".ToDateTimeInPara(zy.CreateDate),
                "@Ip".ToVarCharInPara(zy.Ip),
                "@IMEI".ToVarCharInPara(zy.IMEI),
                "@MobileBrand".ToVarCharInPara(zy.MobileBrand),
                "@SystemType".ToVarCharInPara(zy.SystemType),
                "@Browser".ToVarCharInPara(zy.Browser),
                "@Structed".ToBitInPara(zy.Structed)
                );
            return o == null ? 0 : int.Parse(o.ToString());
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
            return MySqlHelper.ExecuteNonQuery(Util.GetConnectString("EasyZy_Home"), sql);
        }

        /// <summary>
        /// 修改作业状态为设置过答题卡
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public static int UpdateZyStructed(int zyId)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString("EasyZy_Home"),
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
        public static T_ZyStruct GetZyStruct(int zyId)
        {
            T_ZyStruct model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString("EasyZy_Home"),
                "select Id, ZyId, BqNum, SqNum, QuesType, QuesAnswer, CreateDate from T_ZyStruct where ZyId = @ZyId",
                "@ZyId".ToInt32InPara(zyId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_ZyStruct>(dr);
                }
            }
            return model;
        }
    }
}
