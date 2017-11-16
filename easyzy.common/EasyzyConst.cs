using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.common
{
    public class EasyzyConst
    {
        /// <summary>
        /// 上传Word文件所在功能的枚举
        /// </summary>
        public enum WordFunc
        {
            CreateZY = 0
        }

        public static string GetZyNum(int id)
        {
            return "Zy" + id;
        }

        public static int GetZyId(string ZyNum)
        {
            return int.Parse(ZyNum.ToLower().Replace("zy", ""));
        }

        public enum CacheProject
        {
            EasyZy
        }

        public enum CacheCatalog
        {
            User,
            Zy
        }
    }
}
