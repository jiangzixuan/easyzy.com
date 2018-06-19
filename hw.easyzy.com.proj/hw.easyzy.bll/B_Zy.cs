using easyzy.sdk;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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
        /// 根据UserId查找作业列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<dto_Zy> GetZyList(int userId, int pageIndex, int pageSize, out int totalCount)
        {
            List<dto_Zy> list = null;
            using (MySqlDataReader dr = MySqlDBHelper.GetPageReader(Util.GetConnectString(ZyConnString),
                "Id, UserId, ZyName, CourseId, SubjectId, CreateDate, OpenDate, DueDate, Type, Status ",
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
                "select Id, UserId, ZyName, CourseId, SubjectId, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser, OpenDate, DueDate, Type, Status from T_Zy where Id = @Id",
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
                "insert into T_Zy(UserId, ZyName, CourseId, SubjectId, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser, OpenDate, DueDate, Type, Status) values (@UserId, @ZyName, @CourseId, @SubjectId, @CreateDate, @Ip, @IMEI, @MobileBrand, @SystemType, @Browser, @OpenDate, @DueDate, @Type, @Status); select last_insert_id();",
                "@UserId".ToInt32InPara(zy.UserId),
                "@ZyName".ToVarCharInPara(zy.ZyName),
                "@CourseId".ToInt32InPara(zy.CourseId),
                "@SubjectId".ToInt32InPara(zy.SubjectId),
                "@CreateDate".ToDateTimeInPara(zy.CreateDate),
                "@Ip".ToVarCharInPara(zy.Ip),
                "@IMEI".ToVarCharInPara(zy.IMEI),
                "@MobileBrand".ToVarCharInPara(zy.MobileBrand),
                "@SystemType".ToVarCharInPara(zy.SystemType),
                "@Browser".ToVarCharInPara(zy.Browser),
                "@OpenDate".ToDateTimeInPara(zy.OpenDate),
                "@DueDate".ToDateTimeInPara(zy.DueDate),
                "@Type".ToInt32InPara(zy.Type),
                "@Status".ToInt32InPara(zy.Status)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        /// <summary>
        /// 新增Qdb作业试题
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="quesJson"></param>
        /// <returns></returns>
        public static bool AddQdbZyQues(int id, string quesJson)
        {
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(ZyConnString),
                "insert into T_ZyQuestions(Id, QuesJson, CreateDate) values (@Id, @QuesJson, @CreateDate)",
                "@Id".ToInt32InPara(id),
                "@QuesJson".ToVarCharInPara(quesJson),
                "@CreateDate".ToDateTimeInPara(DateTime.Now)
                );
            return i > 0;
        }

        /// <summary>
        /// 仅返回作业试题json信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetQdbZyQues(int id)
        {
            string s = "";
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(ZyConnString),
                "select QuesJson from T_ZyQuestions where Id = @Id",
                "@Id".ToInt32InPara(id));
            if (o != null)
            {
                s = o.ToString();
            }
            return s;
        }
        
        /// <summary>
        /// 修改作业状态
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateZyStatus(int zyId, int status)
        {
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(ZyConnString),
                "update T_Zy set Status = @Status where Id = @Id",
                "@Id".ToInt32InPara(zyId),
                "@Status".ToInt32InPara(status)
                );
            return i > 0;
        }
    }
}
