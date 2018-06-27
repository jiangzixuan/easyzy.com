using easyzy.sdk;
using hw.easyzy.common;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.bll
{
    public class B_User
    {
        private static string UserConnString = "";
        static B_User()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.User, out UserConnString);
        }

        public static T_User GetUser(int id)
        {
            T_User model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, ZyPsd, ZyPrice, ProvinceId, CityId, DistrictId, SchoolId, GradeId, ClassId from T_User where Id = @Id",
                "@Id".ToInt32InPara(id)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_User>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 查询用户被哪些人关注
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int[] GetBeRelatedUser(int userId)
        {
            List<int> result = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select UserId from T_UserRelate where RUserId = @RUserId",
                "@RUserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    result = new List<int>();
                    while (dr.Read())
                    {
                        result.Add(int.Parse(dr[0].ToString()));
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 查询用户关注了哪些人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int[] GetRelatedUser(int userId)
        {
            List<int> result = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select RUserId from T_UserRelate where UserId = @UserId",
                "@UserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    result = new List<int>();
                    while (dr.Read())
                    {
                        result.Add(int.Parse(dr[0].ToString()));
                    }
                }
            }
            return result == null ? null : result.ToArray();
        }
    }
}
