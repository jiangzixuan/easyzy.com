using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.entity
{
    public class T_TextBooks
    {
        
        public int Id { get; set; }
        
        public int CourseId { get; set; }
        
        public string Description { get; set; }
        
        public int GradeId { get; set; }
        
        public string Name { get; set; }
        
        public int Ordinal { get; set; }
        
        public string Term { get; set; }
        
        public int VersionId { get; set; }
        
        public string Volume { get; set; }
    }
}
