using easyzy.sdk;
using hw.easyzy.common;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easyzy.sdk.Const;

namespace hw.easyzy.bll
{
    public class B_ZyRedis
    {
        //缓存有效期(7天）
        private static TimeSpan ts = new TimeSpan(7, 0, 0, 0);

        /// <summary>
        /// 获取作业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T_Zy GetZy(int id)
        {
            T_Zy tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZy, id.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.SelfZy.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<T_Zy>(key);
                    if (tempresult == null)
                    {
                        tempresult = B_Zy.GetZy(id);
                        if (tempresult != null)
                        {
                            client.Set<T_Zy>(key, tempresult, ts);
                        }
                    }
                }
            }
            
            return tempresult;
        }

        /// <summary>
        /// 获取作业结构信息
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        //public static List<T_ZyStruct> GetZyStruct(int zyId)
        //{
        //    List<T_ZyStruct> tempresult = null;

        //    string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZyStruct, zyId.ToString());
        //    using (var client = RedisHelper.GetRedisClient(CacheCatalog.SelfZyStruct.ToString()))
        //    {
        //        if (client != null)
        //        {
        //            tempresult = client.Get<List<T_ZyStruct>>(key);
        //            if (tempresult == null)
        //            {
        //                tempresult = B_Zy.GetZyStruct(zyId);
        //                if (tempresult != null && tempresult.Count > 0)
        //                {
        //                    client.Set<List<T_ZyStruct>>(key, tempresult);
        //                }
        //            }
        //        }
        //    }
            
        //    return tempresult;
        //}

        //public static T_Answer GetZyAnswer(int zyId, string trueName)
        //{
        //    T_Answer tempresult = null;
        //    string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZyAnswer, zyId.ToString() + trueName);
        //    using (var client = RedisHelper.GetRedisClient(CacheCatalog.SelfZyAnswer.ToString()))
        //    {
        //        if (client != null)
        //        {
        //            tempresult = client.Get<T_Answer>(key);
        //            if (tempresult == null)
        //            {
        //                tempresult = B_Zy.GetZyAnswer(zyId, trueName);
        //                if (tempresult != null)
        //                {
        //                    client.Set<T_Answer>(key, tempresult, ts);
        //                }
        //            }
        //        }
        //    }
            
        //    return tempresult;
        //}

        //public static T_Answer GetZyAnswer(int zyId, int studentId)
        //{
        //    T_Answer tempresult = null;
        //    string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZyAnswer, zyId.ToString() + studentId.ToString());
        //    using (var client = RedisHelper.GetRedisClient(CacheCatalog.SelfZyAnswer.ToString()))
        //    {
        //        if (client != null)
        //        {
        //            tempresult = client.Get<T_Answer>(key);
        //            if (tempresult == null)
        //            {
        //                tempresult = B_Zy.GetZyAnswer(zyId, studentId);
        //                if (tempresult != null)
        //                {
        //                    client.Set<T_Answer>(key, tempresult, ts);
        //                }
        //            }
        //        }
        //    }
            
        //    return tempresult;
        //}

        /// <summary>
        /// 修改作业为已建答题卡
        /// </summary>
        /// <param name="zyId"></param>
        //public static void UpdateZyStructed(int zyId)
        //{
        //    T_Zy tempresult = null;
        //    string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZy, zyId.ToString());
        //    using (var client = RedisHelper.GetRedisClient(CacheCatalog.SelfZy.ToString()))
        //    {
        //        if (client != null)
        //        {
        //            tempresult = client.Get<T_Zy>(key);
        //            if (tempresult != null)
        //            {
        //                tempresult.Structed = true;
        //                client.Set<T_Zy>(key, tempresult, ts);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 删除作业缓存
        /// </summary>
        /// <param name="zyId"></param>
        public static void DeleteZyCache(int zyId)
        {
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZy, zyId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.SelfZy.ToString()))
            {
                if (client != null)
                {
                    client.Remove(key);
                }
            }
        }
    }
}
