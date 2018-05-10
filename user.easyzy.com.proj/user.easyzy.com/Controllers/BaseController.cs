﻿using user.easyzy.bll;
using user.easyzy.model.entity;
using System;
using System.Web.Mvc;
using easyzy.sdk;

namespace user.easyzy.com.Controllers
{
    public class BaseController : Controller
    {
        protected int UserId = 0;
        protected T_User User = null;

        public BaseController()
        {
            string u = Util.GetCookie(Const.CookieName_User, Const.CookieVluew_UserId);
            if (!string.IsNullOrEmpty(u))
            {
                UserId = int.Parse(u);
                User = B_UserRedis.GetUser(UserId);
            }
            ViewBag.UserInfo = User;
        }

        #region 因为路由规则问题未解决，暂将公共方法放这里
        public string SaveSuggest(string content, string name, string phone)
        {
            T_Suggestion t = new T_Suggestion() { Name = name, Phone = phone, Content = content, CreateDate = DateTime.Now, Processed = false };
            int i = B_Common.AddSuggestion(t);
            return i > 0 ? "" : "Error";
        }

        public void Exit()
        {
            try
            {
                Util.ClearCookies(Const.CookieName_User);
            }
            catch
            { }
        }
        #endregion
    }
}