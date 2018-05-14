using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace easyzy.sdk
{
    public class UserCookieHelper
    {
        public struct UserCookieModel
        {
            public int _id { get; set; }

            public string _ip { get; set; }

            public long _timestamp { get; set; }

            public int _schoolid { get; set; }

            public int _classid { get; set; }
        }

        /// <summary>
        /// User Cookie 加密
        /// </summary>
        /// <param name="cookieModel">用户Cookie实体</param>
        /// <param name="key">DES加密key</param>
        /// <returns></returns>
        public static string EncryptUserCookie(UserCookieModel cookieModel, string key)
        {
            return(DesUtil.Encrypt3DES_2(string.Concat(cookieModel._id, "\f", cookieModel._ip, "\f", cookieModel._timestamp, "\f", cookieModel._schoolid, "\f", cookieModel._classid), key));
        }

        /// <summary>
        /// User Cookie解密
        /// </summary>
        /// <param name="cookieValue"></param>
        /// <param name="key">DES加密key</param>
        /// <returns></returns>
        public static UserCookieModel DescryptUserCookie(string cookieValue, string key)
        {
            UserCookieModel u = new UserCookieModel();
            if (string.IsNullOrEmpty(cookieValue)) return u;
            
            string s = DesUtil.Decrypt3DES_2(cookieValue, key);
            string[] ss = s.Split('\f');

            u._id = int.Parse(ss[0]);
            u._ip = ss[1];
            u._timestamp = long.Parse(ss[2]);
            u._schoolid = int.Parse(ss[3]);
            u._classid = int.Parse(ss[4]);
            
            return u;
        }
    }
}
