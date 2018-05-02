using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.model.entity
{
    public class T_Suggestion
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Processed { get; set; }
    }
}
