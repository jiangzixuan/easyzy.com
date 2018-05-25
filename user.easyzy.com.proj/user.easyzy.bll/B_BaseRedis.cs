using easyzy.sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.easyzy.model.entity;
using static easyzy.sdk.Const;

namespace user.easyzy.bll
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
            string tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Base, "CL_" + provinceId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<string>(key);
                }
            }
            List<T_City> result = null;
            if (!string.IsNullOrEmpty(tempresult))
            {
                result = JsonConvert.DeserializeObject<List<T_City>>(tempresult);
            }
            else
            {
                result = B_Base.GetCities(provinceId);
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
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

        /// <summary>
        /// 获取区列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static List<T_District> GetDistricts(int cityId)
        {
            string tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Base, "DL_" + cityId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<string>(key);
                }
            }
            List<T_District> result = null;
            if (!string.IsNullOrEmpty(tempresult))
            {
                result = JsonConvert.DeserializeObject<List<T_District>>(tempresult);
            }
            else
            {
                result = B_Base.GetDistricts(cityId);
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
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

        /// <summary>
        /// 获取学校列表
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public static List<T_School> GetSchools(int districtId)
        {
            string tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Base, "SL_" + districtId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<string>(key);
                }
            }
            List<T_School> result = null;
            if (!string.IsNullOrEmpty(tempresult))
            {
                result = JsonConvert.DeserializeObject<List<T_School>>(tempresult);
            }
            else
            {
                result = B_Base.GetSchools(districtId);
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
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

        /// <summary>
        /// 获取学校
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        public static T_School GetSchool(int schoolId)
        {
            string tempresult = null;

            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Base, "SCH_" + schoolId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<string>(key);
                }
            }
            T_School result = null;
            if (!string.IsNullOrEmpty(tempresult))
            {
                result = JsonConvert.DeserializeObject<T_School>(tempresult);
            }
            else
            {
                result = B_Base.GetSchool(schoolId);
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.Base.ToString()))
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
    }
}
