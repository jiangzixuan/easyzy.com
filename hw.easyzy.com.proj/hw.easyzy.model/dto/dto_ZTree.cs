using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_ZTree
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public int pId { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool ischecked { get; set; }
        /// <summary>
        /// 自定属性
        /// </summary>
        public string ntype { get; set; }

        /// <summary>
        /// 节点样式
        /// </summary>
        public string iconSkin { get; set; }

        
    }
}
