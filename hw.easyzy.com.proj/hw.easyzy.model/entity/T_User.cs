using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.entity
{
    public class T_User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string TrueName { get; set; }

        public string Class { get; set; }

        public string Psd { get; set; }

        public string Mobile { get; set; }

        public DateTime FirstLoginDate { get; set; }

        public DateTime CreateDate { get; set; }

        public string Extend1 { get; set; }

        /// <summary>
        /// 作业默认密码
        /// </summary>
        public string ZyPsd { get; set; }

        /// <summary>
        /// 作业收费
        /// </summary>
        public int ZyPrice { get; set; }
    }
}
