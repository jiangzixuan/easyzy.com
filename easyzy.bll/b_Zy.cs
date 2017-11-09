using easyzy.common;
using easyzy.model.entity;
using MySql.Data.MySqlClient;

namespace easyzy.bll
{
    public class b_Zy
    {
        public static T_Zy GetZy(int Id)
        {
            T_Zy model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString("EasyZy_Home"),
                "select Id, UserId, BodyWordPath, BodyHtmlPath, AnswerWordPath, AnswerHtmlPath, CreateDate, Ip, IMEI, MobileBrand, SystemType from T_Zy where Id = @Id",
                "@Id".ToInt32InPara(Id)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_Zy>(dr);
                }
            }
            return model;
        }
    }
}
