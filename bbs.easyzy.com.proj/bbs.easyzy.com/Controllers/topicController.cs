using bbs.easyzy.bll;
using bbs.easyzy.model.dto;
using bbs.easyzy.model.entity;
using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
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

        public ActionResult gettopics(int gradeId, int subjectId, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_Topic> tl = B_Topic.GetTopics(gradeId, subjectId, pageIndex, pageSize, out totalCount);
            if (tl != null)
            {
                foreach (var t in tl)
                {
                    T_User u = B_UserRedis.GetUser(t.UserId);
                    t.UserName = u == null ? "" : u.UserName;
                    t.TrueName = u == null ? "" : u.TrueName;
                    string GradeName = "", SubjectName = "";
                    Const.Grades.TryGetValue(t.GradeId, out GradeName);
                    Const.Subjects.TryGetValue(t.SubjectId, out SubjectName);
                }
            }
            ViewBag.List = tl;
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
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
        [ValidateInput(false)]
        public JsonResult AddTopic(string invites, string title, string topic, string topicText, int gradeId, int subjectId)
        {
            topic = HttpUtility.UrlDecode(topic);
            if (B_StaticObject.swh.HasBadWord(string.Concat(title, topic)))
            {
                return Json(new { status = "1", message = "标题或者内容中含有敏感词汇，不能提交！", value = 0 });
            }
            topicText = HttpUtility.HtmlDecode(HttpUtility.UrlDecode(topicText).Replace("\n", "").Replace("\t", "")).Trim();
            T_Topic t = new T_Topic()
            {
                UserId = UserInfo.Id,
                Invites = invites,
                Title = title,
                TopicContent = ProcessBigPic(topic, 760),
                TopicText = topicText,
                Hit = 0,
                Good = 0,
                ReplyCount = 0,
                GradeId = gradeId,
                SubjectId = subjectId,
                Deleted = false,
                Blocked = false,
                CreateDate = DateTime.Now,
                Ip = ClientUtil.Ip
            };
            int i = B_Topic.AddTopic(t);
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
            dto_Topic t = B_Topic.GetTopic(id);
            if (t != null)
            {
                T_User u = B_UserRedis.GetUser(t.UserId);
                t.UserName = u == null ? "" : u.UserName;
                t.TrueName = u == null ? "" : u.TrueName;
                string GradeName = "", SubjectName = "";
                Const.Grades.TryGetValue(t.GradeId, out GradeName);
                Const.Subjects.TryGetValue(t.SubjectId, out SubjectName);
            }
            ViewBag.TopicInfo = t;
            return View();
        }

        public ActionResult GetReplyList(int id, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            var ReplyList = B_Topic.GetReplyList(id, pageSize, pageIndex, out totalCount);
            if (ReplyList != null)
            {
                foreach (var r in ReplyList)
                {
                    T_User u = B_UserRedis.GetUser(r.UserId);
                    r.UserName = u == null ? "" : u.UserName;
                    r.TrueName = u == null ? "" : u.TrueName;
                }
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            ViewBag.ReplyList = ReplyList;
            return PartialView();
        }

        public JsonResult GetReplyById(int id)
        {
            dto_Reply r = B_Topic.GetReplyById(id);
            if (r != null)
            {
                T_User u = B_UserRedis.GetUser(r.UserId);
                r.UserName = u == null ? "" : u.UserName;
            }
            return Json(r);
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="TopicID"></param>
        /// <param name="Reply"></param>
        [LoginFilterAttribute]
        [ValidateInput(false)]
        public JsonResult Reply(int topicId, string reply)
        {
            if (B_StaticObject.swh.HasBadWord(string.Concat(reply)))
            {
                return Json(new { status = "1", message = "标题或者内容中含有敏感词汇，不能提交！", value = 0 });
            }

            T_Reply m = new T_Reply();
            m.UserId = this.UserInfo.Id;
            m.TopicId = topicId;
            m.ReplyContent = processQuote(ProcessBigPic(reply, 900));
            m.CreateDate = DateTime.Now;
            m.Deleted = false;
            m.Blocked = false;
            m.Ip = ClientUtil.Ip;
            int i = B_Topic.AddReply(m);
            if (i > 0)
            {
                B_Topic.AddTopicReplyCount(topicId);
                return Json(new { status = "0", message = "", value = i });
            }
            else
            {
                return Json(new { status = "1", message = "发表回复失败！", value = 0 });
            }
        }

        public JsonResult AddTopicGoodCount(int topicId)
        {
            if (B_Topic.HasTopicSetGood(topicId, this.UserInfo.Id)) return Json(new { status = "1", message = "已经点赞过！", value = 0 });
            bool isOK = B_Topic.AddTopicGoodCount(topicId, this.UserInfo.Id);
            if (isOK)
            {
                return Json(new { status = "0", message = "", value = 0 });
            }
            else
            {
                return Json(new { status = "1", message = "点赞失败！", value = 0 });
            }
        }

        #region 私有方法
        /// <summary>
        /// 处理引用
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string processQuote(string s)
        {
            string str = s;
            if (s.IndexOf("[/quote]") != -1)
            {
                str = str.Replace("[quote=", "<fieldset><legend>");
                str = str.Replace("的回复:]<br />", "</legend>");
                str = str.Replace("[/quote]", "</fieldset>");
            }
            return str;
        }

        /// <summary>
        /// 把带大图片的img标签加上宽高限制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        private string ProcessBigPic(string s, int maxWidth)
        {
            string picUrl = "";
            string picLastUrl = "";

            string sString = s;
            int intWidth = 0;
            int intHeight = 0;
            int intWidth2 = 0;
            int intHeight2 = 0;
            try
            {
                if (s.IndexOf("<img") != -1)
                {
                    string[] Pics = Regex.Split(s, "src=\"", RegexOptions.IgnoreCase);
                    sString = Pics[0];
                    for (int j = 1; j < Pics.Length; j++)
                    {
                        bool b = Pics[j].StartsWith("http");
                        if (!b)
                        {
                            picUrl = "http://" + System.Web.HttpContext.Current.Request.Url.Host + Pics[j];
                        }
                        else
                        {
                            picUrl = Pics[j];
                        }
                        picLastUrl = picUrl.Substring(picUrl.IndexOf("\""), picUrl.Length - picUrl.IndexOf("\""));

                        picUrl = picUrl.Substring(0, picUrl.IndexOf("\""));

                        using (WebClient webClient = new WebClient())
                        {
                            Byte[] imgdata = webClient.DownloadData(picUrl);
                            using (MemoryStream stream = new MemoryStream(imgdata))
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                                intWidth = img.Width;
                                intHeight = img.Height;
                                stream.Close();
                            }
                        }

                        if (intWidth > maxWidth)
                        {
                            intWidth2 = maxWidth;
                            intHeight2 = intHeight * maxWidth / intWidth;
                        }
                        else
                        {
                            intWidth2 = intWidth;
                            intHeight2 = intHeight;
                        }

                        picLastUrl = "\" width=\"" + intWidth2 + "\" height=\"" + intHeight2 + "\"" + picLastUrl.Substring(2, picLastUrl.Length - 2);
                        sString += ("src=\"" + picUrl + picLastUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("processBigPic " + ex.Message);
            }
            return sString;
        }
        #endregion
    }
}