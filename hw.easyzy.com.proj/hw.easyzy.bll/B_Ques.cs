using easyzy.sdk;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.bll
{
    public class B_Ques
    {
        private static string QuesConnString = "";
        static B_Ques()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Ques, out QuesConnString);
        }

        public static List<T_TextBooks> GetTextBooks()
        {
            List<T_TextBooks> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select Id, CourseId, VersionId, Volume, Ordinal from T_TextBooks order by VersionId, Ordinal"))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_TextBooks>(dr);
                }
            }
            return model;
        }

        public static List<T_TextBookVersions> GetTextBookVersions()
        {
            List<T_TextBookVersions> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select Id, CourseId, Name, Ordinal from T_TextBookVersions order by CourseId, Ordinal"))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_TextBookVersions>(dr);
                }
            }
            return model;
        }
    }
}
