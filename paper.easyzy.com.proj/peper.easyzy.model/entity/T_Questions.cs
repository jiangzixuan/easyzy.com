using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paper.easyzy.model.entity
{
    public class T_Questions
    {
        /// <summary>
        /// 试题Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public int courseid { get; set; }

        /// <summary>
        /// 选择题转化为2、3、4、5、6、7后的题型Id
        /// </summary>
        public int ptypeid { get; set; }

        /// <summary>
        /// 一级题型Id
        /// </summary>
        public int btypeid { get; set; }

        /// <summary>
        /// 题型Id
        /// </summary>
        public int typeid { get; set; }

        /// <summary>
        /// 题型名称
        /// </summary>
        public string typename { get; set; }

        /// <summary>
        /// 难度类型1、2、3、4、5
        /// </summary>
        public int difftype { get; set; }

        /// <summary>
        /// 难度值
        /// </summary>
        public float diff { get; set; }

        /// <summary>
        /// 试卷Id
        /// </summary>
        public int paperid { get; set; }

        /// <summary>
        /// 试卷年份
        /// </summary>
        public int paperyear { get; set; }

        /// <summary>
        /// 试卷类型Id
        /// </summary>
        public int papertypeid { get; set; }

        /// <summary>
        /// 是否有小题
        /// </summary>
        public bool haschildren { get; set; }

        /// <summary>
        /// 试题内容
        /// </summary>
        public string quesbody { get; set; }

        /// <summary>
        /// 试题答案
        /// </summary>
        public string quesanswer { get; set; }

        /// <summary>
        /// 试题解析
        /// </summary>
        public string quesparse { get; set; }

        /// <summary>
        /// 知识点字符串
        /// </summary>
        public string kpoints { get; set; }

        /// <summary>
        /// 章节字符串
        /// </summary>
        public string cpoints { get; set; }

        /// <summary>
        /// 使用次数
        /// </summary>
        public int usagetimes { get; set; }
    }
}
