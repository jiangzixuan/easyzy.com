using easyzy.sdk;
using m.easyzy.model.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easyzy.sdk.Const;

namespace m.easyzy.bll
{
    public class B_ZyRedis
    {
        //缓存有效期(7天）
        private static TimeSpan ts = new TimeSpan(7, 0, 0, 0);

        /// <summary>
        /// 获取作业信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
                        {
                            cl.SetRangeInHash(key, GetZyKeyValuePairs(result));
                            cl.ExpireEntryIn(key, ts);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取作业结构信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<T_ZyStruct> GetZyStruct(int ZyId)
        {
            string tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.ZyStruct, ZyId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.ZyStruct.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<string>(key);
                }
            }
            List<T_ZyStruct> result = null;
            if (!string.IsNullOrEmpty(tempresult))
            {
                result = JsonConvert.DeserializeObject<List<T_ZyStruct>>(tempresult);
            }
            else
            {
                result = B_Zy.GetZyStruct(ZyId);
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.ZyStruct.ToString()))
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

        public static T_Answer GetZyAnswer(int zyId, string trueName)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.ZyAnswer, zyId.ToString() + trueName);
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.ZyAnswer.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.GetAllEntriesFromHash(key);
                }
            }
            T_Answer result = null;
            if (tempresult.Count != 0)
            {
                result = RedisHelper.ConvertDicToEntitySingle<T_Answer>(tempresult);
            }
            else
            {
                result = B_Zy.GetZyAnswer(zyId, trueName);
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.ZyAnswer.ToString()))
                    {
                        if (cl != null)
                        {
                            cl.SetRangeInHash(key, GetZyAnswerKeyValuePairs(result));
                            cl.ExpireEntryIn(key, ts);
                        }
                    }
                }
            }
            return result;
        }

        public static T_Answer GetZyAnswer(int zyId, int studentId)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.ZyAnswer, zyId.ToString() + studentId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.ZyAnswer.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.GetAllEntriesFromHash(key);
                }
            }
            T_Answer result = null;
            if (tempresult.Count != 0)
            {
                result = RedisHelper.ConvertDicToEntitySingle<T_Answer>(tempresult);
            }
            else
            {
                result = B_Zy.GetZyAnswer(zyId, studentId);
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.ZyAnswer.ToString()))
                    {
                        if (cl != null)
                        {
                            cl.SetRangeInHash(key, GetZyAnswerKeyValuePairs(result));
                            cl.ExpireEntryIn(key, ts);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 修改作业为已建答题卡
        /// </summary>
        /// <param name="zyId"></param>
        public static void UpdateZyStructed(int zyId)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Zy, zyId.ToString());
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
                result.Structed = true;
                using (var cl = RedisHelper.GetRedisClient(CacheCatalog.Zy.ToString()))
                {
                    if (cl != null)
                    {
                        cl.SetRangeInHash(key, GetZyKeyValuePairs(result));
                        cl.ExpireEntryIn(key, ts);
                    }
                }
            }
        }

        /// <summary>
        /// 删除作业缓存
        /// </summary>
        /// <param name="zyId"></param>
        public static void DeleteZyCache(int zyId)
        {
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Zy, zyId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Zy.ToString()))
            {
                if (client != null)
                {
                    client.Remove(key);
                }
            }
        }

        static List<KeyValuePair<string, string>> GetZyKeyValuePairs(T_Zy m)
        {
            var result = new List<KeyValuePair<string, string>>() {
                            new KeyValuePair<string, string>("Id",m.Id.ToString()),
                            new KeyValuePair<string, string>("UserId",m.UserId.ToString()),
                            new KeyValuePair<string, string>("BodyWordPath",m.AnswerWordPath.ToString()),
                            new KeyValuePair<string, string>("BodyHtmlPath",m.BodyHtmlPath.ToString()),
                            new KeyValuePair<string, string>("AnswerWordPath",m.AnswerWordPath.ToString()),
                            new KeyValuePair<string, string>("AnswerHtmlPath",m.AnswerHtmlPath.ToString()),
                            new KeyValuePair<string, string>("Ip",m.Ip.ToString()),
                            new KeyValuePair<string, string>("IMEI",m.IMEI.ToString()),
                            new KeyValuePair<string, string>("MobileBrand",m.MobileBrand.ToString()),
                            new KeyValuePair<string, string>("SystemType",m.SystemType.ToString()),
                            new KeyValuePair<string, string>("Browser",m.Browser.ToString()),
                            new KeyValuePair<string, string>("CreateDate",m.CreateDate.ToString()),
                            new KeyValuePair<string, string>("Structed",m.Structed.ToString())
                        };
            return result;
        }

        static List<KeyValuePair<string, string>> GetZyStructKeyValuePairs(T_ZyStruct m)
        {
            var result = new List<KeyValuePair<string, string>>() {
                            new KeyValuePair<string, string>("Id",m.Id.ToString()),
                            new KeyValuePair<string, string>("ZyId",m.ZyId.ToString()),
                            new KeyValuePair<string, string>("BqNum",m.BqNum.ToString()),
                            new KeyValuePair<string, string>("SqNum",m.SqNum.ToString()),
                            new KeyValuePair<string, string>("QuesType",m.QuesType.ToString()),
                            new KeyValuePair<string, string>("QuesAnswer",m.QuesAnswer.ToString()),
                            new KeyValuePair<string, string>("CreateDate",m.CreateDate.ToString())
                        };
            return result;
        }

        static List<KeyValuePair<string, string>> GetZyAnswerKeyValuePairs(T_Answer m)
        {
            var result = new List<KeyValuePair<string, string>>() {
                            new KeyValuePair<string, string>("Id",m.Id.ToString()),
                            new KeyValuePair<string, string>("ZyId",m.ZyId.ToString()),
                            new KeyValuePair<string, string>("StudentId",m.StudentId.ToString()),
                            new KeyValuePair<string, string>("TrueName",m.TrueName.ToString()),
                            new KeyValuePair<string, string>("AnswerJson",m.AnswerJson.ToString()),
                            new KeyValuePair<string, string>("AnswerImg",m.AnswerImg.ToString()),
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
