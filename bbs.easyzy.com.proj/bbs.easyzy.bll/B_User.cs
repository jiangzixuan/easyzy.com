using easyzy.sdk;
using bbs.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace bbs.easyzy.bll
{
    public class B_User
    {
        private static string UserConnString = "";
        static B_User()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.User, out UserConnString);
        }

        public static T_User GetUser(int Id)
        {
            T_User model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, Extend1, ZyPsd, ZyPrice, Class from T_User where Id = @Id",
                "@Id".ToInt32InPara(Id)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_User>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 搜索用户，只返回前10个
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static List<T_User> SearchUser(string keyWords)
        {
            List<T_User> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, Extend1, ZyPsd, ZyPrice, Class from T_User where concat(UserName, TrueName, Class) like '%" + keyWords + "%' limit 20"))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_User>(dr);
                }
            }
            return model;
        }
    }
}
