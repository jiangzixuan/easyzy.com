﻿using easyzy.sdk;
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

        public static T_User GetUser(string userName)
        {
            T_User model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(UserConnString),
                "select Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, Extend1, ZyPsd, ZyPrice, Class from T_User where UserName = @UserName",
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
                "insert into T_User(Id, UserName, TrueName, Psd, Mobile, FirstLoginDate, CreateDate, Extend1, ZyPsd, ZyPrice, Class) values (null, @UserName, @TrueName, @Psd, @Mobile, @FirstLoginDate, @CreateDate, @Extend1, @ZyPsd, @ZyPrice, @Class); select last_insert_id();",
                "@UserName".ToVarCharInPara(zy.UserName),
                "@TrueName".ToVarCharInPara(zy.TrueName),
                "@Psd".ToVarCharInPara(zy.Psd),
                "@Mobile".ToVarCharInPara(zy.Mobile),
                "@FirstLoginDate".ToDateTimeInPara(zy.FirstLoginDate),
                "@CreateDate".ToDateTimeInPara(zy.CreateDate),
                "@Extend1".ToVarCharInPara(zy.Extend1),
                "@ZyPsd".ToVarCharInPara(zy.ZyPsd),
                "@ZyPrice".ToInt32InPara(zy.ZyPrice),
                "@Class".ToVarCharInPara(zy.Class)
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
