using easyzy.common;
using easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.bll
{
    public class B_User
    {
        public static T_User GetUser(int Id)
        {
            T_User model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString("EasyZy_Home"),
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, Extend1 from T_User where Id = @Id",
                "@Id".ToInt32InPara(Id)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_User>(dr);
                }
            }
            return model;
        }

        public static T_User GetUser(string userName)
        {
            T_User model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString("EasyZy_Home"),
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, Extend1 from T_User where UserName = @UserName",
                "@UserName".ToVarCharInPara(userName)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_User>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="zy"></param>
        /// <returns></returns>
        public static int Create(T_User zy)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString("EasyZy_Home"),
                "insert into T_User(Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, Extend1) values (null, @UserName, @TrueName, @Psd, @Mobile, @FirstLoginDate, @CreateDate, @Extend1); select last_insert_id();",
                "@UserName".ToVarCharInPara(zy.UserName),
                "@TrueName".ToVarCharInPara(zy.TrueName),
                "@Psd".ToVarCharInPara(zy.Psd),
                "@Mobile".ToVarCharInPara(zy.Mobile),
                "@FirstLoginDate".ToDateTimeInPara(zy.FirstLoginDate),
                "@CreateDate".ToDateTimeInPara(zy.CreateDate),
                "@Extend1".ToVarCharInPara(zy.Extend1)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }
    }
}
