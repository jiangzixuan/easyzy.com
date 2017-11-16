using easyzy.common;
using easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easyzy.common.EasyzyConst;

namespace easyzy.bll
{
    public class B_ZyRedis
    {
        public static T_Zy GetZy(int Id)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Zy, Id.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Zy.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.GetAllEntriesFromHash(key);
                }
            }
            T_Zy result = null;
            if (tempresult.Count != 0)
            {
                result = RedisHelper.ConvertDicToEntitySingle<T_Zy>(tempresult);
            }
            else
            {
                result = B_Zy.GetZy(Id);
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.Zy.ToString()))
                    {
                        if (cl != null)
                            cl.SetRangeInHash(key, GetKeyValuePairs(result));
                    }
                }
            }
            return result;
        }



        static List<KeyValuePair<string, string>> GetKeyValuePairs(T_Zy m)
        {
            var result = new List<KeyValuePair<string, string>>() {
                            new KeyValuePair<string, string>("Id",m.Id.ToString()),
                            new KeyValuePair<string, string>("UserId",m.UserId.ToString()),
                            new KeyValuePair<string, string>("TrueName",m.BodyWordPath.ToString()),
                            new KeyValuePair<string, string>("BodyWordPath",m.AnswerWordPath.ToString()),
                            new KeyValuePair<string, string>("BodyHtmlPath",m.BodyHtmlPath.ToString()),
                            new KeyValuePair<string, string>("AnswerHtmlPath",m.AnswerHtmlPath.ToString()),//支持多班级
                            new KeyValuePair<string, string>("Ip",m.Ip.ToString()),
                            new KeyValuePair<string, string>("IMEI",m.IMEI.ToString()),
                            new KeyValuePair<string, string>("MobileBrand",m.MobileBrand.ToString()),
                            new KeyValuePair<string, string>("SystemType",m.SystemType.ToString()),
                            new KeyValuePair<string, string>("Browser",m.Browser.ToString()),
                            new KeyValuePair<string, string>("CreateDate",m.CreateDate.ToString())
                        };
            return result;
        }
    }
}
