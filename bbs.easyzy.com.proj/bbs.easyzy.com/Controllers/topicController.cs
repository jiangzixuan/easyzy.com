using bbs.easyzy.bll;
using bbs.easyzy.model.entity;
using easyzy.sdk;
using System;
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

        [LoginFilterAttribute]
        public ActionResult add()
        {
            ViewBag.Grades = Const.Grades;
            ViewBag.Subjects = Const.Subjects;
            return View();
        }

        [LoginFilterAttribute]
        public JsonResult AddTopic(string invites, string title, string topic, string topicText, int gradeId, int subjectId)
        {
            topic = HttpUtility.UrlDecode(topic);
            topicText = HttpUtility.HtmlDecode(HttpUtility.UrlDecode(topicText).Replace("\n", "").Replace("\t", "")).Trim();
            T_Topic t = new T_Topic()
            {
                UserId = UserInfo.Id,
                Invites = invites,
                Title = title,
                TopicContent = topic,
                TopicText = topicText,
                Hit = 0,
                Good = 0,
                ReplyCount = 0,
                GradeId = gradeId,
                SubjectId = subjectId,
                Deleted = false,
                Blocked = false,
                CreateDate = DateTime.Now
            };
            int i = B_Topic.AddTopic(t);
            return Json(new { status = "0", message = "", value = i });
        }

        public ActionResult detail(int id)
        {
            return View();
        }
    }
}