using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_ZyAnswer
    {
        public int QId { get; set; }
        
        public string Answer { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public decimal Point { get; set; }
    }
}
