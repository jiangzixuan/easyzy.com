using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bbs.easyzy.com.Controllers
{
    public class topicController : BaseController
    {
        public ActionResult list()
        {
            return View();
        }

        public ActionResult add()
        {
            ViewBag.Grades = Const.Grades;
            ViewBag.Subjects = Const.Subjects;
            return View();
        }

        public string AddTopic(string title, string topic, string topicText)
        {
            topic = HttpUtility.UrlDecode(topic);
            topicText = HttpUtility.HtmlDecode(HttpUtility.UrlDecode(topicText).Replace("\n", "").Replace("\t", "")).Trim();
            return "";
        }

        public ActionResult detail(int id)
        {
            return View();
        }


    }
}