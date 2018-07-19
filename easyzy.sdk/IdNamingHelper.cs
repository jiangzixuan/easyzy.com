using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.sdk
{
    /// <summary>
    /// 用最简单的方式重新计算对外的Id（先乘再加）
    /// </summary>
    public class IdNamingHelper
    {
        public static long _zytimes = 56375099;
        public static long _zyadds = 478531297;

        public static long _questimes = 45902673;
        public static long _quesadds = 260039487;
        

        public enum IdTypeEnum
        {
            Ques,
            Zy,
            Paper
        }

        public static long Encrypt(IdTypeEnum type, int id)
        {
            long newid = 0;
            if (type == IdTypeEnum.Ques)
            {
                newid = id * _questimes + _quesadds;
            }
            else if (type == IdTypeEnum.Zy || type == IdTypeEnum.Paper)
            {
                newid = id * _zytimes + _zyadds;
            }

            return newid;
        }

        public static int Decrypt(IdTypeEnum type, long id)
        {
            long oldid = 0;
            if (type == IdTypeEnum.Ques)
            {
                oldid = (id - _quesadds) / _questimes;
            }
            else if (type == IdTypeEnum.Zy || type == IdTypeEnum.Paper)
            {
                oldid = (id - _zyadds) / _zytimes;
            }

            return (int)oldid;
        }
        
    }
}
