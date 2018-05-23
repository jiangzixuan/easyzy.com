using user.easyzy.model.dto;
using user.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using easyzy.sdk;

namespace user.easyzy.bll
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
        /// 新建用户
        /// </summary>
        /// <param name="zy"></param>
        /// <returns></returns>
        public static int Create(T_User zy)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(UserConnString),
                "insert into T_User(Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, Extend1, ZyPsd, ZyPrice, ProvinceId, CityId, DistrictId, SchoolId, GradeId, ClassId) values (null, @UserName, @TrueName, @Psd, @Mobile, @FirstLoginDate, @CreateDate, @Extend1, @ZyPsd, @ZyPrice, @ProvinceId, @CityId, @DistrictId, @SchoolId, @GradeId, @ClassId); select last_insert_id();",
                "@UserName".ToVarCharInPara(zy.UserName),
                "@TrueName".ToVarCharInPara(zy.TrueName),
                "@Psd".ToVarCharInPara(zy.Psd),
                "@Mobile".ToVarCharInPara(zy.Mobile),
                "@FirstLoginDate".ToDateTimeInPara(zy.FirstLoginDate),
                "@CreateDate".ToDateTimeInPara(zy.CreateDate),
                "@Extend1".ToVarCharInPara(zy.Extend1),
                "@ZyPsd".ToVarCharInPara(zy.ZyPsd),
                "@ZyPrice".ToInt32InPara(zy.ZyPrice),
                "@ProvinceId".ToInt32InPara(zy.ProvinceId),
                "@CityId".ToInt32InPara(zy.CityId),
                "@DistrictId".ToInt32InPara(zy.DistrictId),
                "@SchoolId".ToInt32InPara(zy.SchoolId),
                "@GradeId".ToInt32InPara(zy.GradeId),
                "@ClassId".ToInt32InPara(zy.ClassId)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        /// <summary>
        /// 修改真实姓名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="trueName"></param>
        /// <returns></returns>
        public static int UpdateTrueName(int userId, string trueName)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "update T_User set TrueName = @TrueName where Id = @UserId",
                "@UserId".ToInt32InPara(userId),
                "@TrueName".ToVarCharInPara(trueName)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        /// <summary>
        /// 修改作业默认密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="zyPsd"></param>
        /// <returns></returns>
        public static int UpdateZyPsd(int userId, string zyPsd)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "update T_User set ZyPsd = @ZyPsd where Id = @UserId",
                "@UserId".ToInt32InPara(userId),
                "@ZyPsd".ToVarCharInPara(zyPsd)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        /// <summary>
        /// 修改班级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userClass"></param>
        /// <returns></returns>
        public static int UpdateClass(int userId, string userClass)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "update T_User set Class = @Class where Id = @UserId",
                "@UserId".ToInt32InPara(userId),
                "@Class".ToVarCharInPara(userClass)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        /// <summary>
        /// 修改首次登陆时间
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="firstLoginDate"></param>
        /// <returns></returns>
        public static int UpdateFirstLoginDate(int userId, DateTime firstLoginDate)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "update T_User set FirstLoginDate = @FirstLoginDate where Id = @UserId",
                "@UserId".ToInt32InPara(userId),
                "@FirstLoginDate".ToDateTimeInPara(firstLoginDate)
                );
            return o == null ? 0 : int.Parse(o.ToString());
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

        public static int AddRelate(int userId, int rUserId, DateTime createDate)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(UserConnString),
                "insert into T_UserRelate(Id, UserId, RUserId, CreateDate) values (null, @UserId, @RUserId, @CreateDate); select last_insert_id();",

                "@UserId".ToInt32InPara(userId),
                "@RUserId".ToInt32InPara(rUserId),
                "@CreateDate".ToDateTimeInPara(createDate)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        public static int CancelRelate(int userId, int rUserId)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "delete from T_UserRelate where UserId = @UserId and RUserId = @RUserId",
                "@UserId".ToInt32InPara(userId),
                "@RUserId".ToInt32InPara(rUserId)
                );
            return o == null ? 0 : int.Parse(o.ToString());
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
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, ZyPsd, ZyPrice, ProvinceId, CityId, DistrictId, SchoolId, GradeId, ClassId from T_User where concat(UserName, TrueName) like '%" + keyWords + "%' limit 20"))
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
