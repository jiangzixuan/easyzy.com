using easyzy.sdk;
using hw.easyzy.common;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easyzy.sdk.Const;

namespace hw.easyzy.bll
{
    public class B_UserRedis
    {
        //缓存有效期(30天）
        private static TimeSpan ts = new TimeSpan(30, 0, 0, 0);
        

        /// <summary>
        /// 根据UserId查询User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static dto_User GetUser(int userId)
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

        private static dto_User TransUserToDtoUser(T_User u)
        {
            dto_User result = new dto_User()
            {
                Id = u.Id,
                UserName = u.UserName,
                Psd = u.Psd,
                Mobile = u.Mobile,
                CreateDate = u.CreateDate,
                FirstLoginDate = u.FirstLoginDate,
                TrueName = u.TrueName,
                ZyPrice = u.ZyPrice,
                ZyPsd = u.ZyPsd,
                ProvinceId = u.ProvinceId,
                CityId = u.CityId,
                DistrictId = u.DistrictId,
                SchoolId = u.SchoolId,
                GradeId = u.GradeId,
                ClassId = u.ClassId
            };
            string pName = "";
            bool b = Const.Provinces.TryGetValue(result.ProvinceId, out pName);
            result.ProvinceName = pName == null ? "" : pName;
            result.CityName = result.CityId == 0 ? "" : B_BaseRedis.GetCities(result.ProvinceId).Find(a => a.CityId == result.CityId).CityName;
            result.DistrictName = result.DistrictId == 0 ? "" : B_BaseRedis.GetDistricts(result.CityId).Find(a => a.DistrictId == result.DistrictId).DistrictName;
            result.SchoolName = result.SchoolId == 0 ? "" : B_BaseRedis.GetSchool(result.SchoolId).SchoolName;
            string gName = "";
            Const.Grades.TryGetValue(result.GradeId, out gName);
            result.GradeName = gName == null ? "" : gName;
            result.ClassName = result.ClassId == 0 ? "" : result.ClassId + "班";
            return result;
        }

        static List<KeyValuePair<string, string>> GetUserKeyValuePairs(dto_User m)
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
