﻿using user.easyzy.common;
using user.easyzy.model.dto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.easyzy.bll
{
    public class B_UserZy
    {
        /// <summary>
        /// 获取老师新建的作业列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<dto_UserZy> GetUserZy(int userId)
        {
            List<dto_UserZy> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(EasyzyConst.ZyConnectStringName),
                "select Id ZyId, UserId, BodyHtmlPath, AnswerHtmlPath, CreateDate, Structed from T_Zy where UserId = @UserId order by CreateDate desc",
                "@UserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_UserZy>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 获取老师新建的作业列表，分页
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<dto_UserZy> GetUserZy(int userId, int pageIndex, int pageSize, out int totalCount)
        {
            List<dto_UserZy> model = null;
            using (MySqlDataReader dr = MySqlDBHelper.GetPageReader(Util.GetConnectString(EasyzyConst.ZyConnectStringName),
                "Id ZyId, UserId, BodyHtmlPath, AnswerHtmlPath, CreateDate, Structed",
                "T_Zy where UserId = @UserId",
                "CreateDate desc",
                pageSize,
                pageIndex,
                out totalCount,
                "@UserId".ToInt32InPara(userId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_UserZy>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 获取学生提交的作业列表
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static List<dto_UserZy> GetSubmitedZy(int studentId)
        {
            List<dto_UserZy> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(EasyzyConst.ZyConnectStringName),
                "select ZyId, CreateDate from T_Answer where StudentId = @StudentId order by CreateDate desc",
                "@StudentId".ToInt32InPara(studentId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_UserZy>(dr);
                }
            }
            return model;
        }

        public static List<dto_UserZy> GetSubmitedZy(int studentId, int pageIndex, int pageSize, out int totalCount)
        {
            List<dto_UserZy> model = null;
            using (MySqlDataReader dr = MySqlDBHelper.GetPageReader(Util.GetConnectString(EasyzyConst.ZyConnectStringName),
                "ZyId, CreateDate",
                "T_Answer where StudentId = @StudentId",
                "CreateDate desc",
                pageSize,
                pageIndex,
                out totalCount,
                "@StudentId".ToInt32InPara(studentId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_UserZy>(dr);
                }
            }
            return model;
        }
    }
}