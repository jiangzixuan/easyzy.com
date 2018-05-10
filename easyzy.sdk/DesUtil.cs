using System;
using System.Text;
using System.Security.Cryptography;

namespace easyzy.sdk
{
    /// <summary>
    /// Des算法相关公共方法
    /// </summary>
    public class DesUtil
    {
        /// <summary>
        /// 加密的key暂写在这里
        /// </summary>
        static string key = "qhklN4eijJe10n'UBNkMe";

        /// <summary>  
        /// 3DES加密  
        /// base64小细节，当使用get请求时，base64生成字符中有“+”号，  
        /// 注意需要转换“%2B”，否则会被替换成空格。POST不存在  
        /// while (str.IndexOf('+') != -1) {  
        ///  str = str.Replace("+","%2B");  
        //  }  
        /// </summary>  
        public static string Encrypt3DES_2(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            try
            {
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                var DES = new TripleDESCryptoServiceProvider();
                DES.Key = encoding.GetBytes(key);
                DES.Mode = CipherMode.ECB;
                ICryptoTransform DESEncrypt = DES.CreateEncryptor();
                byte[] Buffer = encoding.GetBytes(data);
                return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>  
        /// 3DES解密  
        /// </summary>  
        /// <returns>解密串</returns>  
        /// <param name="data">加密串</param>  
        public static string Decrypt3DES_2(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            try
            {
                var DES = new TripleDESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
                DES.Mode = CipherMode.ECB;
                DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                byte[] Buffer = Convert.FromBase64String(data);
                return ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
                LogHelper.Error("Decrypt3DES_2 Error:" + e.Message);
                return string.Empty;
            }
        }
    }
}
