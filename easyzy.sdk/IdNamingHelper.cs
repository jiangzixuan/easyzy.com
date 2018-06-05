using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.sdk
{
    /// <summary>
    /// 用最简单的方式重新计算对外的Id
    /// </summary>
    public class IdNamingHelper
    {
        public static int _zytimes = 375099;
        public static int _zyadds = 657831297;

        public static int _questimes = 902673;
        public static int _quesadds = 264412810;

        public enum IdTypeEnum
        {
            Ques,
            Zy,
            Paper
        }

        public static int Encrypt(IdTypeEnum type, int id)
        {
            int newid = 0;
            if (type == IdTypeEnum.Ques)
            {
                newid = id * _questimes + _quesadds;
            }
            else if(type == IdTypeEnum.Zy)
            {
                newid = id * _zytimes + _zyadds;
            }

            return newid;
        }

        public static int Decrypt(IdTypeEnum type, int id)
        {
            int oldid = 0;
            if (type == IdTypeEnum.Ques)
            {
                oldid = (id - _quesadds) / _questimes;
            }
            else if (type == IdTypeEnum.Zy)
            {
                oldid = (id - _zyadds) / _zytimes;
            }

            return oldid;
        }
        
    }
}
