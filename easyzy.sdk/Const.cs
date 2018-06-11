using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.sdk
{
    /// <summary>
    /// 定义系统中一些常量或者共用、不变的枚举/集合
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
            SubmitAnswer = 0,
            BBSKindEditer = 1
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
            User,    //用户
            SelfZy,  //自传作业
            QdbZy,   //题库作业
            ZyQues,  //作业试题
            Ques,    //试题
            SelfZyStruct,  //自传作业结构
            SelfZyAnswer,  //自传作业答案
            CheckCode,     //验证码
            Base,    //基础数据
            TextBooks,  //教材集合
            TextBookVersions, //教材版本集合
            Catalogs,   //课程下的各书本的目录
            KnowledgePoints

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
            User,
            BBS
        }

        public static Dictionary<DBName, string> DBConnStrNameDic = new Dictionary<DBName, string>()
        {
            { DBName.Base, "EasyZy_Base" },
            { DBName.Ques, "EasyZy_Ques" },
            { DBName.Home, "EasyZy_Home" },
            { DBName.User, "EasyZy_User" },
            { DBName.Zy, "EasyZy_Zy" },
            { DBName.BBS, "EasyZy_BBS" }
        };

        #endregion

        /// <summary>
        /// 选择题题型ID集合
        /// </summary>
        public static readonly List<int> OBJECTIVE_QUES_TYPES = new List<int> { 2, 3, 4, 5, 6, 7 };

        /// <summary>
        /// 学段枚举
        /// </summary>
        public enum StagesEnum
        {
            ALL,
            PRESCHOOL,
            PRIMARY, //小学
            JUNIOR_MIDDLE, //初中
            SENIOR_MIDDLE, //高中
            COLLEGE
        }

        /// <summary>
        /// 知识点数节点类型枚举
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
        /// 省份字典，与数据库一致
        /// </summary>
        public static readonly Dictionary<int, string> Provinces = new Dictionary<int, string> { { 110000, "北京" }, { 120000, "天津" }, { 130000, "河北" }, { 140000, "山西" }, { 150000, "内蒙古" }, { 210000, "辽宁" }, { 220000, "吉林" }, { 230000, "黑龙江" }, { 310000, "上海" }, { 320000, "江苏" }, { 330000, "浙江" }, { 340000, "安徽" }, { 350000, "福建" }, { 360000, "江西" }, { 370000, "山东" }, { 410000, "河南" }, { 420000, "湖北" }, { 430000, "湖南" }, { 440000, "广东" }, { 450000, "广西" }, { 460000, "海南" }, { 500000, "重庆" }, { 510000, "四川" }, { 520000, "贵州" }, { 530000, "云南" }, { 540000, "西藏" }, { 610000, "陕西" }, { 620000, "甘肃" }, { 630000, "青海" }, { 640000, "宁夏" }, { 650000, "新疆" }, { 710000, "台湾" }, { 810000, "香港" }, { 820000, "澳门" } };

        /// <summary>
        /// 年级字典，与数据库一致
        /// </summary>
        public static readonly Dictionary<int, string> Grades = new Dictionary<int, string> { { 1, "一年级" }, { 2, "二年级" }, { 3, "三年级" }, { 4, "四年级" }, { 5, "五年级" }, {6, "六年级" }, { 7, "七年级" }, { 8, "八年级" }, { 9, "九年级" }, { 10, "高一" }, { 11, "高二" }, { 12, "高三" } };
        
        /// <summary>
        /// 学科字典，与数据库一致
        /// </summary>
        public static readonly Dictionary<int, string> Subjects = new Dictionary<int, string> { { 1, "语文" }, { 2, "数学"}, { 3, "英语" }, { 4, "物理" }, { 5, "化学" }, { 6, "生物" }, { 7, "政治" }, { 8, "历史" }, { 9, "地理" }, { 10, "科学" }, { 11, "历史与社会" }, { 12, "信息技术" }, { 13, "音乐" }, { 14, "美术" }, { 15, "体育与健康" }, { 16, "通用技术" }, { 17, "劳动技术" } };
        
        /// <summary>
        /// 易作业各学段支持的课程字典
        /// </summary>
        public static readonly Dictionary<StagesEnum, string> StageCourses = new Dictionary<StagesEnum, string> { { StagesEnum.PRIMARY, "1,2,3" }, { StagesEnum.JUNIOR_MIDDLE, "10,11,12,13,14,15,16,17,18" }, { StagesEnum.SENIOR_MIDDLE, "26,27,28,29,30,31,32,33,34" } };

        /// <summary>
        /// 课程字典
        /// </summary>
        public static readonly Dictionary<int, string> Courses = new Dictionary<int, string>()
        {
            {1, "小学语文" }, {2, "小学数学" }, {3, "小学英语" }, {4, "小学品德" }, {5, "小学科学" }, {6, "小学信息技术" }, {7, "小学音乐" }, {8, "小学美术" }, {9, "小学体育" },
            {10, "初中语文" }, {11, "初中数学" }, {12, "初中英语" }, {13, "初中物理" }, {14, "初中化学" }, {15, "初中生物" }, {16, "初中思想品德" }, {17, "初中历史" }, {18, "初中地理" },{19, "初中科学" },{20, "初中信息技术" },{21, "初中历史与社会" },{22, "初中音乐" },{23, "初中美术" },{24, "初中体育与健康" },{25, "初中劳动技术" },
            {26, "高中语文" }, {27, "高中数学" }, {28, "高中英语" }, {29, "高中物理" }, {30, "高中化学" }, {31, "高中生物" }, {32, "高中政治" }, {33, "高中历史" }, {34, "高中地理" }, {35, "高中信息技术" },{36, "高中音乐" },{37, "高中美术" },{38, "高中体育与健康" },{39, "高中通用技术" },{40, "高中劳动技术" }
        };

        /// <summary>
        /// 课程学科匹配字典，不在此字典的不支持
        /// </summary>
        public static readonly Dictionary<int, int> CourseSubjectMapping = new Dictionary<int, int>()
        {
            { 1, 1}, { 2, 2}, { 3,3},
            { 10,1}, { 11,2}, {12,3 },{13, 4 },{14, 5 },{15, 6 }, {16, 7 },{17, 8 },{18, 9 },
            { 26, 1}, { 27, 2},{28, 3 },{29, 4 },{30, 5 },{31, 6 },{32, 7 },{33, 8 },{34, 9 }
        };
    }
}
