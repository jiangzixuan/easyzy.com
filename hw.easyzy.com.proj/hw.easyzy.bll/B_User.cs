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
        /// 获取被关注用户数，按照年级&班级分组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<dto_RelateGroup> GetGroupedRelatedUser(int userId)
        {
            List<dto_RelateGroup> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select b.GradeId, b.ClassId, count(1) TotalCount from T_UserRelate a, T_User b where a.UserId = b.Id and a.RUserId = @RUserId group by b.GradeId, b.ClassId",
                "@RUserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_RelateGroup>(dr);
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
    }
}
