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
        /// 查询作业
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T_Zy GetZy(int id)
        {
            T_Zy model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select Id, UserId, ZyName, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser, OpenDate, DueDate, Type from T_Zy where Id = @Id",
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
                "insert into T_Zy(UserId, ZyName, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser, OpenDate, DueDate, Type) values (@UserId, @ZyName, @CreateDate, @Ip, @IMEI, @MobileBrand, @SystemType, @Browser, @OpenDate, @DueDate, @Type); select last_insert_id();",
                "@UserId".ToInt32InPara(zy.UserId),
                "@ZyName".ToVarCharInPara(zy.ZyName),
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
    }
}
