using easyzy.sdk;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace hw.easyzy.bll
{
    public class B_Ques
    {
        private static string QuesConnString = "";
        static B_Ques()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Ques, out QuesConnString);
        }

        public static List<T_CatalogNodes> GetCatalogNodes(int textbookId)
        {
            List<T_CatalogNodes> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select Id, Name, Ordinal, ParentId, TextBookId, Type from T_CatalogNodes a where a.TextBookId = @TextBookId order by ParentId, Ordinal",
                "@TextBookId".ToInt32InPara(textbookId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_CatalogNodes>(dr);
                }
            }
            return model;
        }

        public static List<T_KnowledgePoints> GetKnowledgePoints(int courseId)
        {
            List<T_KnowledgePoints> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(QuesConnString),
                "select Id, CourseId, Name, Ordinal, ParentId, Type from T_KnowledgePoints a where a.CourseId = @CourseId order by ParentId, Ordinal",
                "@CourseId".ToInt32InPara(courseId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_KnowledgePoints>(dr);
                }
            }
            return model;
        }
    }
}
