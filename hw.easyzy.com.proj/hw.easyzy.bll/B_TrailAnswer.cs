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
    }
}
