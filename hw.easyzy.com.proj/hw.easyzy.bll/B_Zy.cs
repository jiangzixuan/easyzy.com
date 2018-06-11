using easyzy.sdk;
using hw.easyzy.common;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace hw.easyzy.bll
{
    public class B_Zy
    {
        private static string ZyConnString = "";
        static B_Zy()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Zy, out ZyConnString);
        }

        /// <summary>
        /// 查找我新建的作业列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<dto_Zy> GetMyZy(int userId, int pageIndex, int pageSize, out int totalCount)
        {
            List<dto_Zy> list = null;
            using (MySqlDataReader dr = MySqlDBHelper.GetPageReader(Util.GetConnectString(ZyConnString),
                "Id, UserId, ZyName, SubjectId, CreateDate, OpenDate, DueDate, Type ",
                "T_Zy where UserId = @UserId",
                "Id desc",
                pageSize,
                pageIndex, 
                out totalCount,
                "@UserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    list = MySqlDBHelper.ConvertDataReaderToEntityList<dto_Zy>(dr);
                }
            }
            return list;
        }

        /// <summary>
        /// 查询作业
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T_Zy GetZy(int id)
        {
            T_Zy model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select Id, UserId, ZyName, SubjectId, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser, OpenDate, DueDate, Type from T_Zy where Id = @Id",
                "@Id".ToInt32InPara(id)))
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
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(ZyConnString),
                "insert into T_Zy(UserId, ZyName, SubjectId, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser, OpenDate, DueDate, Type) values (@UserId, @ZyName, @SubjectId, @CreateDate, @Ip, @IMEI, @MobileBrand, @SystemType, @Browser, @OpenDate, @DueDate, @Type); select last_insert_id();",
                "@UserId".ToInt32InPara(zy.UserId),
                "@ZyName".ToVarCharInPara(zy.ZyName),
                "@SubjectId".ToInt32InPara(zy.SubjectId),
                "@CreateDate".ToDateTimeInPara(zy.CreateDate),
                "@Ip".ToVarCharInPara(zy.Ip),
                "@IMEI".ToVarCharInPara(zy.IMEI),
                "@MobileBrand".ToVarCharInPara(zy.MobileBrand),
                "@SystemType".ToVarCharInPara(zy.SystemType),
                "@Browser".ToVarCharInPara(zy.Browser),
                "@OpenDate".ToDateTimeInPara(zy.OpenDate),
                "@DueDate".ToDateTimeInPara(zy.DueDate),
                "@Type".ToInt32InPara(zy.Type)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        /// <summary>
        /// 新增Qdb作业试题
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="quesJson"></param>
        /// <returns></returns>
        public static bool AddQdbZyQues(int zyId, string quesJson)
        {
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(ZyConnString),
                "insert into T_ZyQuestions(Id, QuesJson, CreateDate) values (@Id, @QuesJson, @CreateDate)",
                "@Id".ToInt32InPara(zyId),
                "@QuesJson".ToVarCharInPara(quesJson),
                "@CreateDate".ToDateTimeInPara(DateTime.Now)
                );
            return i > 0;
        }

        /// <summary>
        /// 查找作业每个班提交数量
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public static Dictionary<int, int> GetZySubmitCountByClass(int zyId)
        {
            Dictionary<int, int> d = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select GradeId, ClassId, count(1) from T_Answer where Id = @Id group by GradeId, ClassId",
                "@Id".ToInt32InPara(zyId)))
            {
                if (dr != null && dr.HasRows)
                {
                    d = new Dictionary<int, int>();
                    while (dr.Read())
                    {
                        string s = string.Concat(dr[0], dr[1].ToString().PadLeft(2, '0'));
                        d.Add(int.Parse(s), int.Parse(dr[2].ToString()));
                    }
                }
            }
            return d;
        }
    }
}
