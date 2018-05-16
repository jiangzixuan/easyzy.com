using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace bbs.easyzy.bll
{
    public class B_StaticObject
    {
        public static SensitiveWordHelper swh = new SensitiveWordHelper(GetMapPath("/badwords.txt"));

        private static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
        }
    }

    
}
