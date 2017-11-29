using easyzy.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easyzy.common.EasyzyConst;

namespace easyzy.bll
{
    /// <summary>
    /// 存放验证码
    /// </summary>
    public class B_CheckCodeRedis
    {
        //缓存有效期(5分钟）
        private static TimeSpan ts = new TimeSpan(0, 5, 0);

        public static string GetCheckCode(string token)
        {
            string result = "";
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.CheckCode, token);
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.CheckCode.ToString()))
            {
                if (client != null)
                {
                    result = client.Get<string>(key);
                }
            }
            
            return result;
        }

        public static void SetCheckCode(string token, string checkCode)
        {
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.CheckCode, token);
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.CheckCode.ToString()))
            {
                if (client != null)
                {
                    client.Set<string>(key, checkCode, ts);
                }
            }
        }
    }
}
