using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.sdk
{
    /// <summary>
    /// 定义系统中一些常量或者公共的枚举
    /// </summary>
    public class Const
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
        public enum DBName
        {
            Home,
            Base,
            Ques,
            Zy,
            User
        }

        public static Dictionary<DBName, string> DBConnStrNameDic = new Dictionary<DBName, string>()
        {
            { DBName.Base, "EasyZy_Base" },
            { DBName.Ques, "EasyZy_Ques" },
            { DBName.Home, "EasyZy_Home" },
            { DBName.User, "EasyZy_User" },
            { DBName.Zy, "EasyZy_Zy" }
        };

        #endregion

        #region Cookie名称

        public static string CookieName_User = "easyzy.user";
        public static string CookieVluew_UserId = "userid";

        #endregion

        /// <summary>
        /// 试题类型 选择题
        /// </summary>
        public static readonly int[] QUES_TYPES = new int[] { 2, 3, 4, 5, 6, 7 };

        /// <summary>
        /// 学段枚举
        /// </summary>
        public enum StagesEnum
        {
            ALL,
            PRESCHOOL,
            PRIMARY,
            JUNIOR_MIDDLE,
            SENIOR_MIDDLE,
            COLLEGE
        }

        /// <summary>
        /// 知识点类型枚举
        /// </summary>
        public enum KnowledgePointTypeEnum
        {
            /// <summary>
            /// 结点
            /// </summary>
            NODE,
            /// <summary>
            /// 知识点
            /// </summary>
            KNOWLEDGE_POINT,
            /// <summary>
            /// 考点
            /// </summary>
            TESTING_POINT,
            /// <summary>
            /// 章节
            /// </summary>
            CATALOG_NODES
        }

        /// <summary>
        /// 地区字典
        /// </summary>
        public static readonly Dictionary<int, string> Provinces = new Dictionary<int, string> { { 110000, "北京" }, { 120000, "天津" }, { 130000, "河北" }, { 140000, "山西" }, { 150000, "内蒙古" }, { 210000, "辽宁" }, { 220000, "吉林" }, { 230000, "黑龙江" }, { 310000, "上海" }, { 320000, "江苏" }, { 330000, "浙江" }, { 340000, "安徽" }, { 350000, "福建" }, { 360000, "江西" }, { 370000, "山东" }, { 410000, "河南" }, { 420000, "湖北" }, { 430000, "湖南" }, { 440000, "广东" }, { 450000, "广西" }, { 460000, "海南" }, { 500000, "重庆" }, { 510000, "四川" }, { 520000, "贵州" }, { 530000, "云南" }, { 540000, "西藏" }, { 610000, "陕西" }, { 620000, "甘肃" }, { 630000, "青海" }, { 640000, "宁夏" }, { 650000, "新疆" }, { 710000, "台湾" }, { 810000, "香港" }, { 820000, "澳门" } };

    }
}
