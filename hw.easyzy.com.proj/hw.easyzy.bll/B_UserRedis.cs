using easyzy.sdk;
using hw.easyzy.common;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            dto_User tempresult = null;
            T_User u = null;
            string key = RedisHelper.GetEasyZyRedisKey(Const.CacheCatalog.User, userId.ToString());
            using (var client = RedisHelper.GetRedisClient(Const.CacheCatalog.User.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<dto_User>(key);
                    if (tempresult == null)
                    {
                        u = B_User.GetUser(userId);
                        if (u != null)
                        {
                            tempresult = TransUserToDtoUser(u);
                            client.Set(key, tempresult, ts);
                        }
                    }
                }
            }

            return tempresult;
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
        
    }
}
