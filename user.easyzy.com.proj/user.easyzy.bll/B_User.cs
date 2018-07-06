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
        public static bool UpdateTrueName(int userId, string trueName)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "update T_User set TrueName = @TrueName where Id = @UserId",
                "@UserId".ToInt32InPara(userId),
                "@TrueName".ToVarCharInPara(trueName)
                );
            return o == null ? false : true;
        }

        /// <summary>
        /// 修改作业默认密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="zyPsd"></param>
        /// <returns></returns>
        public static bool UpdateZyPsd(int userId, string zyPsd)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "update T_User set ZyPsd = @ZyPsd where Id = @UserId",
                "@UserId".ToInt32InPara(userId),
                "@ZyPsd".ToVarCharInPara(zyPsd)
                );
            return o == null ? false : true;
        }

        /// <summary>
        /// 修改班级，【省、市、区、学校】如果传-1，则不修改
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="provinceId"></param>
        /// <param name="cityId"></param>
        /// <param name="districtId"></param>
        /// <param name="schoolId"></param>
        /// <param name="gradeId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static bool UpdateSchool(int userId, int provinceId, int cityId, int districtId, int schoolId)
        {
            List<MySqlParameter> pl = new List<MySqlParameter>();
            
            pl.Add(new MySqlParameter("@ProvinceId", provinceId));
            pl.Add(new MySqlParameter("@CityId", cityId));
            pl.Add(new MySqlParameter("@DistrictId", districtId));
            pl.Add(new MySqlParameter("@SchoolId", schoolId));
            pl.Add(new MySqlParameter("@UserId", userId));

            string sql = "update T_User set ProvinceId = @ProvinceId, CityId = @CityId, DistrictId = @DistrictId, SchoolId = @SchoolId where Id = @UserId";
            
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString), sql, pl.ToArray());
            return i > 0;
        }

        public static bool UpdateClass(int userId, int gradeId, int classId)
        {
            List<MySqlParameter> pl = new List<MySqlParameter>();

            pl.Add(new MySqlParameter("@GradeId", gradeId));
            pl.Add(new MySqlParameter("@ClassId", classId));
            pl.Add(new MySqlParameter("@UserId", userId));

            string sql = "update T_User set GradeId = @GradeId, ClassId = @ClassId where Id = @UserId";

            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString), sql, pl.ToArray());
            return i > 0;
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

        public static bool AddRelate(int userId, int rUserId, DateTime createDate)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(UserConnString),
                "insert into T_UserRelate(Id, UserId, RUserId, CreateDate) values (null, @UserId, @RUserId, @CreateDate); select last_insert_id();",

                "@UserId".ToInt32InPara(userId),
                "@RUserId".ToInt32InPara(rUserId),
                "@CreateDate".ToDateTimeInPara(createDate)
                );
            return o == null ? false : true;
        }

        /// <summary>
        /// 取消关注某人
        /// </summary>
        /// <param name="userId">关注人</param>
        /// <param name="rUserId">被关注人</param>
        /// <returns></returns>
        public static bool CancelRelate(int userId, int rUserId)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "delete from T_UserRelate where UserId = @UserId and RUserId = @RUserId",
                "@UserId".ToInt32InPara(userId),
                "@RUserId".ToInt32InPara(rUserId)
                );
            return o == null ? false : true;
        }

        /// <summary>
        /// 删除关注我的人
        /// </summary>
        /// <param name="userId">被关注人</param>
        /// <param name="rUserId">关注人</param>
        /// <returns></returns>
        public static bool DeleteRelate(int userId, int rUserId)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "delete from T_UserRelate where UserId = @UserId and RUserId = @RUserId",
                "@UserId".ToInt32InPara(rUserId),
                "@RUserId".ToInt32InPara(userId)
                );
            return o == null ? false : true;
        }

        /// <summary>
        /// 搜索用户，只返回前10个
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static List<T_User> SearchUser(string keyWords, int exceptUserId)
        {
            List<T_User> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, ZyPsd, ZyPrice, ProvinceId, CityId, DistrictId, SchoolId, GradeId, ClassId from T_User where concat(UserName, TrueName) like '%" + keyWords + "%' and Id <> @ExceptUserId limit 20",
                "@ExceptUserId".ToInt32InPara(exceptUserId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_User>(dr);
                }
            }
            return model;
        }

        public static dto_ModifyRequest GetModifyRequest(int userId)
        {
            dto_ModifyRequest model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserId, FromSchoolId, ToSchoolId, Reason, Status, CreateDate from T_ModifyRequest where UserId = @UserId and Status = 0 limit 1",
                "@UserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<dto_ModifyRequest>(dr);
                }
            }
            return model;
        }

        public static bool CancelModifyRequest(int id)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "update T_ModifyRequest set Status = 1 where Id = @Id",
                "@Id".ToInt32InPara(id)
                );
            return o == null ? false : true;
        }

        public static int AddModifyRequest(T_ModifyRequest mr)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "insert into T_ModifyRequest(UserId, FromSchoolId, ToSchoolId, Reason, Status, CreateDate) values(@UserId, @FromSchoolId, @ToSchoolId, @Reason, @Status, @CreateDate)",
                "@UserId".ToInt32InPara(mr.UserId),
                "@FromSchoolId".ToInt32InPara(mr.FromSchoolId),
                "@ToSchoolId".ToInt32InPara(mr.ToSchoolId),
                "@Status".ToInt32InPara(mr.Status),
                "@Reason".ToVarCharInPara(mr.Reason),
                "@CreateDate".ToDateTimeInPara(mr.CreateDate)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        public static T_UserExtend GetUserExtend(int userId)
        {
            T_UserExtend model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select UserId, Locked from T_UserExtend where UserId = @UserId",
                "@UserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_UserExtend>(dr);
                }
            }
            return model;
        }

        public static List<T_UserExtend> GetUserExtends(int[] uIds)
        {
            if (uIds.Length == 0) return null;
            string userIds = string.Join(",", uIds);
            List<T_UserExtend> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select UserId, Locked from T_UserExtend where UserId in (" + userIds + ")"))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_UserExtend>(dr);
                }
            }
            return model;
        }

        public static bool UpdateUserExtend(T_UserExtend ue)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "update T_UserExtend set Locked = @Locked where UserId = @UserId",
                "@Locked".ToBitInPara(ue.Locked),
                "@UserId".ToInt32InPara(ue.UserId)
                );
            return o == null ? false : true;
        }

        public static bool AddUserExtend(T_UserExtend ue)
        {
            object o = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(UserConnString),
                "insert into T_UserExtend(UserId, Locked) values(@UserId, @Locked)",
                "@UserId".ToInt32InPara(ue.UserId),
                "@Locked".ToBitInPara(ue.Locked)
                );
            return o == null ? false : true;
        }

        /// <summary>
        /// 查找一个班的所有同学
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="gradeId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static List<T_User> GetClassmates(int schoolId, int gradeId, int classId)
        {
            List<T_User> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, ZyPsd, ZyPrice, ProvinceId, CityId, DistrictId, SchoolId, GradeId, ClassId from T_User where SchoolId = @SchoolId and GradeId = @GradeId and ClassId = @ClassId",
                "@SchoolId".ToInt32InPara(schoolId),
                "@GradeId".ToInt32InPara(gradeId),
                "@ClassId".ToInt32InPara(classId)))
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
