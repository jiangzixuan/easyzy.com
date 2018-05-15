using bbs.easyzy.bll;
using bbs.easyzy.model.entity;
using easyzy.sdk;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 打开添加话题页
        /// </summary>
        /// <returns></returns>
        [LoginFilterAttribute]
        public ActionResult add()
        {
            ViewBag.Grades = Const.Grades;
            ViewBag.Subjects = Const.Subjects;
            return View();
        }

        /// <summary>
        /// 添加话题Ajax提交
        /// </summary>
        /// <param name="invites"></param>
        /// <param name="title"></param>
        /// <param name="topic"></param>
        /// <param name="topicText"></param>
        /// <param name="gradeId"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
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
            //插入邀请表记录
            return Json(new { status = "0", message = "", value = i });
        }

        /// <summary>
        /// 查找要@的人
        /// </summary>
        /// <param name="invites"></param>
        /// <returns></returns>
        public JsonResult GetSuggests(string invites)
        {
            List<T_User> ul = B_User.SearchUser(invites);
            
            return ul == null? Json(new List<T_User>()):Json(ul);
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult detail(int id)
        {
            return View();
        }
    }
}