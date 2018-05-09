using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace easyzy.sdk
{
    /// <summary>
    /// 跟客户端相关的公共方法
    /// </summary>
    public class ClientUtil
    {
        public static string Ip
        {
            get { return GetUserIP(); }
            
        }

        public static string IMEI
        {
            get { return ""; }
            
        }

        public static string MobileBrand
        {
            get { return ""; }
            
        }

        

        #region 私有方法

        /// <summary>
        /// 获取公网IP，通过外网访问，无法获取内网地址
        /// </summary>
        /// <returns></returns>
        private static string GetUserIP()
        {
            string User_IP;
            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    User_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    User_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取用户ID异常：" + ex.Message);
                return "127.0.0.1";
            }
            return User_IP;
        }
        #endregion

    }
}
