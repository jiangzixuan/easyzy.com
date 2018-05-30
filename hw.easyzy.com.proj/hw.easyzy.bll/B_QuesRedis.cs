using easyzy.sdk;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easyzy.sdk.Const;

namespace hw.easyzy.bll
{
    public class B_QuesRedis
    {
        //缓存有效期(500天）
        private static TimeSpan ts = new TimeSpan(500, 0, 0, 0);

        public static List<T_TextBooks> GetTextBooks()
        {
            string tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.TextBooks, "0");
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.TextBooks.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<string>(key);
                }
            }
            List<T_TextBooks> result = null;
            if (!string.IsNullOrEmpty(tempresult))
            {
                result = JsonConvert.DeserializeObject<List<T_TextBooks>>(tempresult);
            }
            else
            {
                result = B_Ques.GetTextBooks();
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.TextBooks.ToString()))
                    {
                        if (cl != null)
                        {
                            cl.Set<string>(key, JsonConvert.SerializeObject(result), ts);
                        }
                    }
                }
            }
            return result;
        }

        public static List<T_TextBookVersions> GetTextBookVersions()
        {
            string tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.TextBookVersions, "0");
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.TextBookVersions.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<string>(key);
                }
            }
            List<T_TextBookVersions> result = null;
            if (!string.IsNullOrEmpty(tempresult))
            {
                result = JsonConvert.DeserializeObject<List<T_TextBookVersions>>(tempresult);
            }
            else
            {
                result = B_Ques.GetTextBookVersions();
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.TextBooks.ToString()))
                    {
                        if (cl != null)
                        {
                            cl.Set<string>(key, JsonConvert.SerializeObject(result), ts);
                        }
                    }
                }
            }
            return result;
        }

        public static List<T_TextBookVersions> GetTextBookVersions(int courseId)
        {
            int[] tbvlist = GetTextBooks().FindAll(a => a.CourseId == courseId).Select(b => b.VersionId).Distinct().ToArray();
            return GetTextBookVersions().FindAll(a => tbvlist.Contains(a.Id));
        }

        public static List<T_TextBooks> GetTextBooks(int versionId)
        {
            return GetTextBooks().FindAll(a => a.VersionId == versionId);
        }
    }
}
