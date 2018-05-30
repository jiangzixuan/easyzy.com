using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_User : T_User
    {
        public string ProvinceName { get; set; }

        public string CityName { get; set; }

        public string DistrictName { get; set; }

        public string SchoolName { get; set; }

        public string GradeName { get; set; }

        public string ClassName { get; set; }

        public bool Locked { get; set; }
    }
}
