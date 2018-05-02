using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.common
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

        public enum ImgFunc
        {
            SubmitAnswer = 0
        }

        #region 作业编号命名规则
        public static string GetZyNum(int id)
        {
            return "Zy" + id;
        }

        public static int GetZyId(string ZyNum)
        {
            return int.Parse(ZyNum.ToLower().Replace("zy", ""));
        }
        #endregion

        /// <summary>
        /// 缓存Id中的项目级
        /// </summary>
        public enum CacheProject
        {
            EasyZy
        }

        /// <summary>
        /// 缓存Id中的分类级
        /// </summary>
        public enum CacheCatalog
        {
            User,
            Zy,
            ZyStruct,
            ZyAnswer,
            CheckCode //验证码
        }

        /// <summary>
        /// 图片后缀名过滤集
        /// </summary>
        public static string[] ImgPattern = new string[] { "jpg", "jpeg", "png", "bmp" };

        /// <summary>
        /// 用户名过滤集
        /// </summary>
        public static string[] UserNameFilter = new string[] { "system", "admin", "sysadmin", "administrator" };

        #region  数据库连接字符串名称
        /// <summary>
        /// 作业库连接字符串名称
        /// </summary>
        public static string ZyConnectStringName = "EasyZy_Home";

        /// <summary>
        /// 用户库连接字符串名称
        /// </summary>
        public static string UserConnectStringName = "EasyZy_Home";

        #endregion

        #region Cookie名称

        public static string CookieName_User = "easyzy.user";
        public static string CookieVluew_UserId = "userid";

        #endregion
    }
}
