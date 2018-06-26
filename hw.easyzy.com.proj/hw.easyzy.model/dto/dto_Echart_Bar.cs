using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    /// <summary>
    /// 显示饼图传递的参数Model
    /// </summary>
    public class dto_Echart_Bar
    {
        public List<string> x { get; set; }

        public List<string> y { get; set; }

        public List<DateTime> o { get; set; }
    }

    public class dto_Echart_Bar2
    {
        public List<string> category { get; set; }

        public List<string> optiona { get; set; }

        public List<string> optionb { get; set; }

        public List<string> optionc { get; set; }

        public List<string> optiond { get; set; }
    }

    public class dto_Echart_Bar3
    {
        public List<dto_Echart_Bar3_Data> data { get; set; }
    }

    public class dto_Echart_Bar3_Data
    {
        public int value { get; set; }

        public string name { get; set; }
    }
}
