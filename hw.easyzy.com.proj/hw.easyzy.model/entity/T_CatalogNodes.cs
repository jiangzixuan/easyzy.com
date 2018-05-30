using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.entity
{
    public class T_CatalogNodes
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public int Ordinal { get; set; }

        public int TextBookId { get; set; }

        public string Type { get; set; }
    }
}
