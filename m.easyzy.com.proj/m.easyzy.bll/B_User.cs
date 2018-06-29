using easyzy.sdk;
using m.easyzy.model.dto;
using m.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace m.easyzy.bll
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

        public static T_User GetUser(string userName)
        {
            T_User model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, ZyPsd, ZyPrice, ProvinceId, CityId, DistrictId, SchoolId, GradeId, ClassId from T_User where UserName = @UserName",
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
        /// 修改首次登陆时间
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="firstLoginDate"></param>
        /// <returns></returns>
        public static bool UpdateFirstLoginDate(int userId, DateTime firstLoginDate)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "update T_User set FirstLoginDate = @FirstLoginDate where Id = @UserId",
                "@UserId".ToInt32InPara(userId),
                "@FirstLoginDate".ToDateTimeInPara(firstLoginDate)
                );
            return o == null ? false : true;
        }

        /// <summary>
        /// 获取关注的用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<dto_RelateUser> GetRelateUser(int userId)
        {
            List<dto_RelateUser> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserId, RUserId, CreateDate from T_UserRelate where UserId = @UserId",
                "@UserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_RelateUser>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 获取被关注用户列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<dto_RelateUser> GetBeRelatedUser(int userId)
        {
            List<dto_RelateUser> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserId, RUserId, CreateDate from T_UserRelate where RUserId = @RUserId",
                "@RUserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_RelateUser>(dr);
                }
            }
            return model;
        }

        public static bool CancelRelate(int userId, int rUserId)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "delete from T_UserRelate where UserId = @UserId and RUserId = @RUserId",
                "@UserId".ToInt32InPara(userId),
                "@RUserId".ToInt32InPara(rUserId)
                );
            return o == null ? false : true;
        }
    }
}
