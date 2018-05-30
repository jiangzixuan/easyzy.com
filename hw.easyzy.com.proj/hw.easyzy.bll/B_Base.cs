using easyzy.sdk;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.bll
{
    public class B_Base
    {
        private static string BaseConnString = "";

        static B_Base()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Base, out BaseConnString);
        }

        public static List<T_City> GetCities(int provinceId)
        {
            List<T_City> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(BaseConnString),
                "select CityId, CityName from T_City where ProvinceId = @ProvinceId",
                "@ProvinceId".ToInt32InPara(provinceId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_City>(dr);
                }
            }
            return model;
        }

        public static List<T_District> GetDistricts(int cityId)
        {
            List<T_District> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(BaseConnString),
                "select DistrictId, DistrictName from T_District where CityId = @CityId",
                "@CityId".ToInt32InPara(cityId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_District>(dr);
                }
            }
            return model;
        }

        public static List<T_School> GetSchools(int districtId)
        {
            List<T_School> model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(BaseConnString),
                "select SchoolId, SchoolName from T_School where DistrictId = @DistrictId",
                "@DistrictId".ToInt32InPara(districtId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<T_School>(dr);
                }
            }
            return model;
        }

        public static T_School GetSchool(int schoolId)
        {
            T_School model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(BaseConnString),
                "select SchoolId, SchoolName from T_School where SchoolId = @SchoolId",
                "@SchoolId".ToInt32InPara(schoolId)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_School>(dr);
                }
            }
            return model;
        }
    }
}
