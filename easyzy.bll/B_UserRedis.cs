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
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.User, userName.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
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
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
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
        public static T_User GetUser(int userId)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
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
                result = B_User.GetUser(userId);
                if (result != null)
                {
                    using (var cl = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
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

        public static void UpdateTrueName(int userId, string trueName)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
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
                result.TrueName = trueName;
                using (var cl = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
                {
                    if (cl != null)
                    {
                        cl.SetRangeInHash(key, GetUserKeyValuePairs(result));
                        cl.ExpireEntryIn(key, ts);
                    }
                }
            }
        }

        public static void UpdateZyPsd(int userId, string zyPsd)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
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
                result.ZyPsd = zyPsd;
                using (var cl = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
                {
                    if (cl != null)
                    {
                        cl.SetRangeInHash(key, GetUserKeyValuePairs(result));
                        cl.ExpireEntryIn(key, ts);
                    }
                }
            }
        }

        public static void UpdateClass(int userId, string userClass)
        {
            Dictionary<string, string> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
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
                result.Class = userClass;
                using (var cl = RedisHelper.GetRedisClient(CacheCatalog.User.ToString()))
                {
                    if (cl != null)
                    {
                        cl.SetRangeInHash(key, GetUserKeyValuePairs(result));
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
                            new KeyValuePair<string, string>("Extend1",m.Extend1.ToString()),
                            new KeyValuePair<string, string>("ZyPsd",m.ZyPsd.ToString()),
                            new KeyValuePair<string, string>("ZyPrice",m.ZyPrice.ToString()),
                            new KeyValuePair<string, string>("Class",m.Class.ToString())
                        };
            return result;
        }
    }
}
