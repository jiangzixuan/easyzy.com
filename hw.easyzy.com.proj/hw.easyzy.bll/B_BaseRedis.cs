using easyzy.sdk;
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
    public class B_BaseRedis
    {
        //缓存有效期(500天）
        private static TimeSpan ts = new TimeSpan(500, 0, 0, 0);

        /// <summary>
        /// 获取市列表
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public static List<T_City> GetCities(int provinceId)
        {
            List<T_City> tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Base, "CL_" + provinceId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<List<T_City>>(key);
                    if (tempresult == null)
                    {
                        tempresult = B_Base.GetCities(provinceId);
                        if (tempresult != null && tempresult.Count > 0)
                        {
                            client.Set<List<T_City>>(key, tempresult, ts);
                        }
                    }
                }
            }
            
            return tempresult;
        }

        /// <summary>
        /// 获取区列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static List<T_District> GetDistricts(int cityId)
        {
            List<T_District> tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Base, "DL_" + cityId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<List<T_District>>(key);
                    if (tempresult == null)
                    {
                        tempresult = B_Base.GetDistricts(cityId);
                        if (tempresult != null && tempresult.Count > 0)
                        {
                            client.Set<List<T_District>>(key, tempresult, ts);
                        }
                    }
                }
            }
            
            return tempresult;
        }

        /// <summary>
        /// 获取学校列表
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public static List<T_School> GetSchools(int districtId)
        {
            List<T_School> tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Base, "SL_" + districtId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<List<T_School>>(key);
                    if (tempresult == null)
                    {
                        tempresult = B_Base.GetSchools(districtId);
                        if (tempresult != null && tempresult.Count > 0)
                        {
                            client.Set<List<T_School>>(key, tempresult, ts);
                        }
                    }
                }
            }
            
            return tempresult;
        }

        /// <summary>
        /// 获取学校
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        public static T_School GetSchool(int schoolId)
        {
            T_School tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Base, "SCH_" + schoolId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<T_School>(key);
                    if (tempresult == null)
                    {
                        tempresult = B_Base.GetSchool(schoolId);
                        if (tempresult != null)
                        {
                            client.Set<T_School>(key, tempresult, ts);
                        }
                    }
                }
            }
            
            return tempresult;
        }
    }
}
