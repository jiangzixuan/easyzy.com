using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.entity
{
    public class T_TextBookVersions
    {
        public int Id { get; set; }
        
        public int CourseId { get; set; }
        
        public string Description { get; set; }
        
        public int FamilyId { get; set; }
        
        public bool InUse { get; set; }
        
        public string Name { get; set; }
        
        public int Ordinal { get; set; }
        
        public int Year { get; set; }
    }
}
