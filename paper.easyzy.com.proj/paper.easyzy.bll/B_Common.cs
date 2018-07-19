using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paper.easyzy.bll
{
    public class B_Common
    {
        public static Dictionary<int, string> GetCoursesByStage(Const.StagesEnum stage)
        {
            Dictionary<int, string> cd = new Dictionary<int, string>();
            string c = "", d = "";

            Const.StageCourses.TryGetValue(stage, out c);
            foreach (var i in c.Split(','))
            {
                Const.Courses.TryGetValue(int.Parse(i), out d);
                cd.Add(int.Parse(i), d.Substring(2));
            }

            return cd;
        }

        public static Dictionary<int, string> GetPaperTypes()
        {
            return Const.PaperTypes;
        }

        public static Dictionary<int, string> GetProvinces()
        {
            return Const.Provinces;
        }

        
    }
}
