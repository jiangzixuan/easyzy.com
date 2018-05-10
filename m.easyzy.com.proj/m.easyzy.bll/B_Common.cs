using easyzy.sdk;
using m.easyzy.common;
using m.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m.easyzy.bll
{
    /// <summary>
    /// 周边功能Bll方法
    /// </summary>
    public class B_Common
    {
        private static string HomeConnString = "";

        static B_Common()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Home, out HomeConnString);
        }
        public static int AddSuggestion(T_Suggestion s)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(HomeConnString),
                "insert into T_Suggestion(Id, Name, Phone, Content, CreateDate, Processed) values (null, @Name, @Phone, @Content, @CreateDate, @Processed); select last_insert_id();",
                "@Name".ToVarCharInPara(s.Name),
                "@Phone".ToVarCharInPara(s.Phone),
                "@Content".ToVarCharInPara(s.Content),
                "@CreateDate".ToDateTimeInPara(s.CreateDate),
                "@Processed".ToBitInPara(s.Processed)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }
    }
}
