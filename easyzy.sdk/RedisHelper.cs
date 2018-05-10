using ServiceStack;
using ServiceStack.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static easyzy.sdk.Const;

namespace easyzy.sdk
{
    /// <summary>
    /// redis访问帮助类
    /// </summary>
    public class RedisHelper
    {
        private const string RedisConfigName = "RedisConfig.xml";

        private static readonly ConcurrentDictionary<string, IRedisClient> RedisCatelogDictionary;

        static RedisHelper()
        {
            RedisCatelogDictionary = new ConcurrentDictionary<string, IRedisClient>();

        }

        private static RedisModel GetRedisModel(string catelog)
        {
            RedisModel r = null;
            string path = AppDomain.CurrentDomain.BaseDirectory + "/" + RedisConfigName;
            if (!File.Exists(path)) return null;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            
            XmlNode xn = doc.SelectSingleNode("root");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xnr in xnl)
            {
                XmlElement xe = (XmlElement)xnr;
                if (xe.GetAttribute("catelog") == catelog)
                {
                    r = new RedisModel();
                    r.Ip = xe.GetAttribute("ip");
                    r.Port = int.Parse(xe.GetAttribute("port"));
                    r.Pwd = xe.GetAttribute("pwd");
                    r.Db = long.Parse(xe.GetAttribute("db"));
                    break;
                }
            }
            return r;
        }

        public static IRedisClient GetRedisClient(string catelog)
        {
            IRedisClient result;
            bool b = RedisCatelogDictionary.TryGetValue(catelog, out result);
            if (!b)
            {
                RedisModel rm = GetRedisModel(catelog);
                //单实例方式，PooledRedisClientManager为集群方式
                result = new RedisClient(rm.Ip, rm.Port, rm.Pwd, rm.Db);
                RedisCatelogDictionary.TryAdd(catelog, result);
            }
            return result;
        }

        /// <summary>
        /// CacheProject = EasyZy
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetEasyZyRedisKey(CacheCatalog cc, string key)
        {
            return "C_" + CacheProject.EasyZy + "_" + cc + "_" + key;
        }

        public static T ConvertDicToEntitySingle<T>(Dictionary<string, string> dic) where T : new()
        {
            if (dic == null)
            {
                return default(T);
            }
            return dic.ToJson().FromJson<T>();

        }

        public static List<T> ConvertDicToEntitySingle<T>(List<Dictionary<string, string>> dic) where T : new()
        {
            if (dic == null)
            {
                return new List<T>();
            }
            List<T> result = new List<T>();
            foreach (var d in dic)
            {
                result.Add(ConvertDicToEntitySingle<T>(d));
            }
            return result;
        }
    }

    public class RedisModel
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public string Pwd { get; set; }

        public long Db { get; set; }
    }
}
