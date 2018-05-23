using user.easyzy.model.entity;
using System;
using System.Collections.Generic;
using easyzy.sdk;
using user.easyzy.model.dto;
using AutoMapper;
using Newtonsoft.Json;

namespace user.easyzy.bll
{
    public class B_UserRedis
    {
        //缓存有效期(30天）
        private static TimeSpan ts = new TimeSpan(30, 0, 0, 0);

        /// <summary>
        /// 根据UserName查询User，应该只会在以下场景使用
        /// 1、注册用户，判断用户名是否存在
        /// 2、登录
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static T_User GetUser(string userName)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(Const.CacheCatalog.User, userName.ToString());
            using (var client = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.GetAllEntriesFromHash(key);
                }
            }
            T_User result = null;
            if (tempresult.Count != 0)
            {
                result = RedisHelper.ConvertDicToEntitySingle<T_User>(tempresult);
            }
            else
            {
                result = B_User.GetUser(userName);

                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
                    {
                        if (cl != null)
                        {
                            cl.SetRangeInHash(key, GetUserKeyValuePairs(result));
                            cl.ExpireEntryIn(key, ts);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据UserId查询User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static dto_User GetUser(int userId)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(Const.CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.GetAllEntriesFromHash(key);
                }
            }
            dto_User result = null;
            if (tempresult.Count != 0)
            {
                result = RedisHelper.ConvertDicToEntitySingle<dto_User>(tempresult);
            }
            else
            {
                T_User tmp = B_User.GetUser(userId);
                if (tmp == null) return null;
                result = TransUserToDtoUser(tmp);
                
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
                    {
                        if (cl != null)
                        {
                            cl.SetRangeInHash(key, GetDtoUserKeyValuePairs(result));
                            cl.ExpireEntryIn(key, ts);
                        }
                    }
                }
            }
            return result;
        }

        private static dto_User TransUserToDtoUser(T_User u)
        {
            dto_User result = new dto_User() { Id = u.Id, UserName = u.UserName, Psd = u.Psd, Mobile = u.Mobile, CreateDate = u.CreateDate, FirstLoginDate = u.FirstLoginDate, TrueName = u.TrueName, ZyPrice = u.ZyPrice, ZyPsd = u.ZyPsd };
            string pName = "";
            bool b = Const.Provinces.TryGetValue(result.ProvinceId, out pName);
            result.ProvinceName = pName == null ? "" : pName;
            result.CityName = result.CityId == 0 ? "" : B_BaseRedis.GetCities(result.ProvinceId).Find(a => a.CityId == result.CityId).CityName;
            result.DistrictName = result.DistrictId == 0 ? "" : B_BaseRedis.GetDistricts(result.CityId).Find(a => a.DistrictId == result.DistrictId).DistrictName;
            result.SchoolName = result.SchoolId == 0 ? "" : B_BaseRedis.GetSchools(result.DistrictId).Find(a => a.SchoolId == result.SchoolId).SchoolName;
            string gName = "";
            Const.Grades.TryGetValue(result.GradeId, out gName);
            result.GradeName = gName == null ? "" : gName;
            result.ClassName = result.ClassId == 0 ? "" : result.ClassId + "班";
            return result;
        }

        public static void UpdateTrueName(int userId, string trueName)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(Const.CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.GetAllEntriesFromHash(key);
                }
            }

            dto_User result = null;
            if (tempresult.Count != 0)
            {
                result = RedisHelper.ConvertDicToEntitySingle<dto_User>(tempresult);
                result.TrueName = trueName;
                using (var cl = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
                {
                    if (cl != null)
                    {
                        cl.SetRangeInHash(key, GetDtoUserKeyValuePairs(result));
                        cl.ExpireEntryIn(key, ts);
                    }
                }
            }
        }

        public static void UpdateZyPsd(int userId, string zyPsd)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(Const.CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.GetAllEntriesFromHash(key);
                }
            }

            dto_User result = null;
            if (tempresult.Count != 0)
            {
                result = RedisHelper.ConvertDicToEntitySingle<dto_User>(tempresult);
                result.ZyPsd = zyPsd;
                using (var cl = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
                {
                    if (cl != null)
                    {
                        cl.SetRangeInHash(key, GetDtoUserKeyValuePairs(result));
                        cl.ExpireEntryIn(key, ts);
                    }
                }
            }
        }

        public static void ReloadUserCache(int userId)
        {
            dto_User result = null;
            T_User tmp = B_User.GetUser(userId);
            if (tmp == null) return;
            result = TransUserToDtoUser(tmp);
            string key = RedisHelper.GetEasyZyRedisKey(Const.CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
            {
                if (client != null)
                {
                    client.SetRangeInHash(key, GetDtoUserKeyValuePairs(result));
                    client.ExpireEntryIn(key, ts);
                }
            }
            
        }

        public static void UpdateFirstLoginDate(int userId, DateTime firstLoginDate)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(Const.CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.GetAllEntriesFromHash(key);
                }
            }

            dto_User result = null;
            if (tempresult.Count != 0)
            {
                result = RedisHelper.ConvertDicToEntitySingle<dto_User>(tempresult);
                result.FirstLoginDate = firstLoginDate;
                using (var cl = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
                {
                    if (cl != null)
                    {
                        cl.SetRangeInHash(key, GetDtoUserKeyValuePairs(result));
                        cl.ExpireEntryIn(key, ts);
                    }
                }
            }
        }

        static List<KeyValuePair<string, string>> GetUserKeyValuePairs(T_User m)
        {
            var result = new List<KeyValuePair<string, string>>() {
                            new KeyValuePair<string, string>("Id",m.Id.ToString()),
                            new KeyValuePair<string, string>("UserName",m.UserName.ToString()),
                            new KeyValuePair<string, string>("TrueName",m.TrueName.ToString()),
                            new KeyValuePair<string, string>("Psd",m.Psd.ToString()),
                            new KeyValuePair<string, string>("Mobile",m.Mobile.ToString()),
                            new KeyValuePair<string, string>("FirstLoginDate",m.FirstLoginDate.ToString()),
                            new KeyValuePair<string, string>("CreateDate",m.CreateDate.ToString()),
                            new KeyValuePair<string, string>("ZyPsd",m.ZyPsd.ToString()),
                            new KeyValuePair<string, string>("ZyPrice",m.ZyPrice.ToString()),
                            new KeyValuePair<string, string>("ProvinceId",m.ProvinceId.ToString()),
                            new KeyValuePair<string, string>("CityId",m.CityId.ToString()),
                            new KeyValuePair<string, string>("DistrictId",m.DistrictId.ToString()),
                            new KeyValuePair<string, string>("SchoolId",m.SchoolId.ToString()),
                            new KeyValuePair<string, string>("GradeId",m.GradeId.ToString()),
                            new KeyValuePair<string, string>("ClassId",m.ClassId.ToString())
                        };
            return result;
        }

        static List<KeyValuePair<string, string>> GetDtoUserKeyValuePairs(dto_User m)
        {
            var result = new List<KeyValuePair<string, string>>() {
                            new KeyValuePair<string, string>("Id",m.Id.ToString()),
                            new KeyValuePair<string, string>("UserName",m.UserName.ToString()),
                            new KeyValuePair<string, string>("TrueName",m.TrueName.ToString()),
                            new KeyValuePair<string, string>("Psd",m.Psd.ToString()),
                            new KeyValuePair<string, string>("Mobile",m.Mobile.ToString()),
                            new KeyValuePair<string, string>("FirstLoginDate",m.FirstLoginDate.ToString()),
                            new KeyValuePair<string, string>("CreateDate",m.CreateDate.ToString()),
                            new KeyValuePair<string, string>("ZyPsd",m.ZyPsd.ToString()),
                            new KeyValuePair<string, string>("ZyPrice",m.ZyPrice.ToString()),
                            new KeyValuePair<string, string>("ProvinceId",m.ProvinceId.ToString()),
                            new KeyValuePair<string, string>("ProvinceName",m.ProvinceName),
                            new KeyValuePair<string, string>("CityId",m.CityId.ToString()),
                            new KeyValuePair<string, string>("CityName",m.CityName),
                            new KeyValuePair<string, string>("DistrictId",m.DistrictId.ToString()),
                            new KeyValuePair<string, string>("DistrictName",m.DistrictName),
                            new KeyValuePair<string, string>("SchoolId",m.SchoolId.ToString()),
                            new KeyValuePair<string, string>("SchoolName",m.SchoolName),
                            new KeyValuePair<string, string>("GradeId",m.GradeId.ToString()),
                            new KeyValuePair<string, string>("GradeName",m.GradeName),
                            new KeyValuePair<string, string>("ClassId",m.ClassId.ToString()),
                            new KeyValuePair<string, string>("ClassName",m.ClassName)
                        };
            return result;
        }
    }
}
