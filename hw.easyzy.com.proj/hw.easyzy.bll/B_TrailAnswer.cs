using easyzy.sdk;
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
    /// <summary>
    /// 试用作业答案T_TrailAnswer表，同一个作业StudentId全为0
    /// </summary>
    public class B_TrailAnswer
    {
        private static string ZyConnString = "";
        static B_TrailAnswer()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Zy, out ZyConnString);
        }

        /// <summary>
        /// 获取作业提交数
        /// </summary>
        /// <param name="zyIds"></param>
        /// <returns></returns>
        public static Dictionary<int, int> GetZySubmitStudentCount(int[] zyIds)
        {
            if (zyIds.Length == 0) return null;
            Dictionary<int, int> d = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select ZyId, count(1) c from T_TrailAnswer where ZyId in (" + string.Join(",", zyIds) + ") and Submited = 1 group by ZyId"))
            {
                if (dr != null && dr.HasRows)
                {
                    d = new Dictionary<int, int>();
                    while (dr.Read())
                    {
                        d.Add(int.Parse(dr[0].ToString()), int.Parse(dr[1].ToString()));
                    }
                }
            }
            return d;
        }

        /// <summary>
        /// 答案表已有数据的情况下，修改图片数组
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="userId"></param>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static bool AddZyImg(int zyId, int userId, string imgPath)
        {
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(ZyConnString),
                "update T_TrailAnswer set AnswerImg = concat(AnswerImg , ',', @imgPath) where ZyId = @ZyId and StudentId = @StudentId",
                "@ZyId".ToInt32InPara(zyId),
                "@StudentId".ToInt32InPara(userId),
                "@imgPath".ToVarCharInPara(imgPath)
                );
            return i > 0;
        }

        /// <summary>
        /// 提交答案
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool InsertZyAnswer(T_Answer a)
        {
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(ZyConnString),
                "insert into T_TrailAnswer(ZyId, ZyType, StudentId, AnswerJson, AnswerImg, Submited, CreateDate, Ip, IMEI, MobileBrand, SystemType, Browser) values (@ZyId, @ZyType, @StudentId, @AnswerJson, @AnswerImg, @Submited, @CreateDate, @Ip, @IMEI, @MobileBrand, @SystemType, @Browser);",
                "@ZyId".ToInt32InPara(a.ZyId),
                "@ZyType".ToInt32InPara(a.ZyType),
                "@StudentId".ToInt32InPara(a.StudentId),
                "@AnswerJson".ToVarCharInPara(a.AnswerJson),
                "@AnswerImg".ToVarCharInPara(a.AnswerImg),
                "@Submited".ToBitInPara(a.Submited),
                "@CreateDate".ToDateTimeInPara(a.CreateDate),
                "@Ip".ToVarCharInPara(a.Ip),
                "@IMEI".ToVarCharInPara(a.IMEI),
                "@MobileBrand".ToVarCharInPara(a.MobileBrand),
                "@SystemType".ToVarCharInPara(a.SystemType),
                "@Browser".ToVarCharInPara(a.Browser)
                );
            return i > 0;
        }

        /// <summary>
        /// 修改作业的AnswerJson并提交作业
        /// </summary>
        /// <param name="zyId"></param>
        /// <param name="userId"></param>
        /// <param name="answerJson"></param>
        /// <returns></returns>
        public static bool UpdateAnswerJson(int zyId, int userId, string answerJson)
        {
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(ZyConnString),
                "update T_TrailAnswer set CreateDate = now(), AnswerJson = @AnswerJson, Submited = 1 where ZyId = @ZyId and StudentId = @StudentId",
                "@ZyId".ToInt32InPara(zyId),
                "@StudentId".ToInt32InPara(userId),
                "@AnswerJson".ToVarCharInPara(answerJson)
                );
            return i > 0;
        }

        public static List<dto_Answer> GetAnswers(int zyId)
        {
            List<dto_Answer> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(ZyConnString),
                "select Id, ZyId, ZyType, StudentId, AnswerJson, AnswerImg, Submited, CreateDate from T_TrailAnswer where ZyId = @ZyId and Submited = 1",
                "@ZyId".ToInt32InPara(zyId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_Answer>(dr);
                }
            }
            return model;
        }
        
    }
}
