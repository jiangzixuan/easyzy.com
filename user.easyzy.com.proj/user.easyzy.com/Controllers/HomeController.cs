using user.easyzy.bll;
using user.easyzy.model.dto;
using user.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using easyzy.sdk;

namespace user.easyzy.com.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Reg()
        {
            //打开获取一个token，作为redis存放验证码的key
            string Token = UniqueObjectID.GenerateStrNewId();
            ViewBag.Token = Token;
            return View();
        }

        /// <summary>
        /// 显示验证码，并记录redis
        /// </summary>
        /// <param name="token"></param>
        public void ShowCheckCode(string token)
        {
            string checkCode = GenerateCheckCode();
            CreateCheckCodeImage(checkCode);
            B_CheckCodeRedis.SetCheckCode(token, checkCode);
        }

        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string IsUserNameExists(string userName)
        {
            if (Const.UserNameFilter.Contains(userName)) return "1";
            T_User u = B_User.GetUser(userName);
            if (u == null)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="token"></param>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public string IsCheckCodeCorrect(string token, string checkCode)
        {
            string CorrectCode = B_CheckCodeRedis.GetCheckCode(token);
            return CorrectCode.ToLower() == checkCode.ToLower() ? "0" : "1";
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public JsonResult RegistUser(string userName, string passWord, string token, string checkCode)
        {
            string CorrectCode = B_CheckCodeRedis.GetCheckCode(token);
            dto_AjaxJsonResult<bool> result = new dto_AjaxJsonResult<bool>();
            if (checkCode.ToLower() != CorrectCode.ToLower())
            {
                result.code = AjaxResultCodeEnum.Error;
                result.message = "验证码错误";
                result.data = false;
                return Json(result);
            }
            if (IsUserNameExists(userName) == "1")
            {
                result.code = AjaxResultCodeEnum.Error;
                result.message = "用户名已被使用";
                result.data = false;
                return Json(result);
            }

            T_User u = new T_User()
            {
                UserName = userName,
                TrueName = "",
                Psd = Util.MD5(passWord),
                Mobile = "",
                FirstLoginDate = DateTime.Parse("2000-01-01 00:00:00"),
                CreateDate = DateTime.Now,
                Extend1 = passWord,
                ZyPsd = "",
                ZyPrice = 0,
                ProvinceId = 0,
                CityId = 0,
                DistrictId = 0,
                SchoolId = 0,
                GradeId = 0,
                ClassId = 0
            };
            int i = B_User.Create(u);
            if (i > 0)
            {
                result.code = AjaxResultCodeEnum.Success;
                result.message = "";
                result.data = true;
            }
            else
            {
                result.code = AjaxResultCodeEnum.Error;
                result.message = "入库失败";
                result.data = false;
                
            }
            return Json(result);
        }

        public ActionResult Login(string from)
        {
            ViewBag.From = string.IsNullOrEmpty(from) ? "http://easyzy.com" : HttpUtility.UrlDecode(from);
            return View();
        }

        public string UserLogin(string userName, string passWord, string isAutoLogin)
        {
            T_User u = B_User.GetUser(userName);
            if (u == null) return "1";
            if (u.Psd == Util.MD5(passWord))
            {
                //如果未登录过，则修改首次登陆时间
                if (u.FirstLoginDate == DateTime.Parse("2000-01-01 00:00:00"))
                {
                    DateTime dtFLD = DateTime.Now;
                    if (B_User.UpdateFirstLoginDate(u.Id, dtFLD))
                    {
                        B_UserRedis.UpdateFirstLoginDate(u.Id, dtFLD);
                    }
                }
                DateTime dt = DateTime.MinValue;
                if (isAutoLogin == "1")
                {
                    dt = DateTime.Now.AddDays(30);
                }
                UserCookieHelper.UserCookieModel m = new UserCookieHelper.UserCookieModel() { _id = u.Id, _uname=u.UserName, _ip = ClientUtil.Ip, _timestamp = Util.GetTimeStamp() };
                string uidentity = UserCookieHelper.EncryptUserCookie(m, Util.GetAppSetting("DesKey"));
                
                Util.SetCookie("easyzy.user", "useridentity", uidentity, dt);
                
                return "0";
            }
            else
            {
                return "1";
            }
        }

        #region 验证码相关
        /// <summary>
        /// 生成长度为4的验证码
        /// </summary>
        /// <returns></returns>
        private string GenerateCheckCode()
        {
            char code;
            string checkCode = String.Empty;
            int validateCodeCount = 4;
            // 验证码的字符集，去掉了一些容易混淆的字符
            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            System.Random random = new Random();

            for (int i = 0; i < validateCodeCount; i++)
            {
                code = character[random.Next(character.Length)];
                
                checkCode += code;
            }
            return checkCode;
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="checkCode"></param>
        private void CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 15.0 + 40)), 23);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);

            try
            {
                //生成随机生成器  
                Random random = new Random();

                //清空图片背景色  
                g.Clear(System.Drawing.Color.White);

                //画图片的背景噪音线  
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Silver), x1, y1, x2, y2);
                }

                System.Drawing.Font font = new System.Drawing.Font("Arial", 14, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Color.Blue, System.Drawing.Color.DarkRed, 1.2f, true);

                int cySpace = 16;
                for (int i = 0; i < 4; i++)
                {
                    g.DrawString(checkCode.Substring(i, 1), font, brush, (i + 1) * cySpace, 1);
                }

                //画图片的前景噪音点  
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, System.Drawing.Color.FromArgb(random.Next()));
                }

                //画图片的边框线  
                g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        #endregion
    }
}