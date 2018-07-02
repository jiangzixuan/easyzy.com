using easyzy.sdk;
using m.easyzy.common;
using m.easyzy.model.dto;
using m.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;

namespace m.easyzy.bll
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
        /// <param name="lastId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<dto_Zy> GetZyList(int userId, int lastId, int count)
        {
            List<dto_Zy> list = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select Id, UserId, ZyName, CourseId, SubjectId, CreateDate, OpenDate, DueDate, Type, Status from T_Zy where UserId = @UserId and Id < @Id order by Id desc limit @Count",
                "@UserId".ToInt32InPara(userId),
                "@Id".ToInt32InPara(lastId),
                "@Count".ToInt32InPara(count)))
            {
                if (dr != null && dr.HasRows)
                {
                    list = MySqlDBHelper.ConvertDataReaderToEntityList<dto_Zy>(dr);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据一组Userid获取他们新建的作业
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="lastId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<dto_Zy> GetZyList(int[] userIds, int lastId, int count)
        {

            if (userIds == null || userIds.Length == 0)
            {
                return null;
            }
            List<dto_Zy> list = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select Id, UserId, ZyName, CourseId, SubjectId, CreateDate, OpenDate, DueDate, Type, Status from T_Zy where UserId in (" + string.Join(",", userIds) + ") and Id < @Id order by Id desc limit @Count",
                "@Id".ToInt32InPara(lastId),
                "@Count".ToInt32InPara(count)))
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
