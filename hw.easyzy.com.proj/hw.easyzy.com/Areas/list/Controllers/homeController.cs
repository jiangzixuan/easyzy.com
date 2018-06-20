using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.list.Controllers
{
    public class homeController : baseController
    {
        public ActionResult Index()
        {
            ViewBag.UserId = UserId;
            return View();
        }

        /// <summary>
        /// 获取试用的作业列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetTrialZy(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_Zy> list = B_Zy.GetZyList(0, pageIndex, pageSize, out totalCount);
            if (list != null)
            {
                List<dto_RelateGroup> rgl = null;
                Dictionary<int, int> d = B_Answer.GetZySubmitStudentCount(list.Select(a => a.Id).ToArray());

                foreach (var l in list)
                {
                    //隐藏真实Id
                    l.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Zy, l.Id);
                    
                    string subName = "";
                    Const.Subjects.TryGetValue(l.SubjectId, out subName);
                    l.SubjectName = subName;
                    l.TypeName = l.Type == 0 ? "题库" : "自传";
                    int c = 0;
                    if (d != null)
                    {
                        d.TryGetValue(l.Id, out c);
                    }
                    rgl = new List<dto_RelateGroup>();
                    rgl.Add(new dto_RelateGroup() { ClassId = 0, ClassName = "试用班", GradeId = 0, GradeName = "", SubmitCount = c, TotalCount = 0 });
                    l.ClassSubmitInfo = rgl;
                    l.Id = 0;
                }
            }
            
            ViewBag.ZyList = list;
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }

        /// <summary>
        /// 获取我新建的作业列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetMyZy(int pageIndex, int pageSize)
        {
            
            int totalCount = 0;
            List<dto_Zy> list = null;
            if (UserId != 0)
            {
                list = B_Zy.GetZyList(UserId, pageIndex, pageSize, out totalCount);
                if (list != null)
                {
                    List<dto_RelateGroup> rgl = B_User.GetGroupedRelatedUser(UserId);

                    foreach (var l in list)
                    {
                        string subName = "";
                        Const.Subjects.TryGetValue(l.SubjectId, out subName);
                        l.SubjectName = subName;
                        l.TypeName = l.Type == 0 ? "题库" : "自传";

                        List<int> subl = B_Answer.GetZySubmitStudents(l.Id);
                        subl.ForEach(a =>
                        {
                            dto_User u = B_UserRedis.GetUser(a);
                            var r = rgl.FirstOrDefault(b => b.GradeId == u.GradeId && b.ClassId == u.ClassId);
                            if (r != null) r.SubmitCount += 1;
                        });

                        l.ClassSubmitInfo = rgl;
                    }
                }
            }
            ViewBag.ZyList = list;
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }

        /// <summary>
        /// 关闭作业
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public JsonResult CloseZy(long zyId)
        {
            dto_AjaxJsonResult<string> r = new dto_AjaxJsonResult<string>();
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);
            if (zy.Status == 2)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "作业已删除，不能关闭！";
                r.data = "";
                return Json(r);
            }
            else if (zy.Status == 1)
            {
                r.code = AjaxResultCodeEnum.Success;
                r.message = "";
                r.data = "";
                return Json(r);
            }
            else
            {
                bool isok = B_Zy.UpdateZyStatus(id, 1);
                if (isok)
                {
                    B_ZyRedis.UpdateZyStatus(id, 1);
                    r.code = AjaxResultCodeEnum.Success;
                    r.message = "";
                    r.data = "";
                    return Json(r);
                }
                else
                {
                    r.code = AjaxResultCodeEnum.Error;
                    r.message = "关闭失败！";
                    r.data = "";
                    return Json(r);
                }
            }
        }

        /// <summary>
        /// 删除作业
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public JsonResult DeleteZy(long zyId)
        {
            dto_AjaxJsonResult<string> r = new dto_AjaxJsonResult<string>();
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);
            if (zy.Status == 2)
            {
                r.code = AjaxResultCodeEnum.Success;
                r.message = "";
                r.data = "";
                return Json(r);
            }
            else
            {
                bool isok = B_Zy.UpdateZyStatus(id, 2);
                if (isok)
                {
                    B_ZyRedis.UpdateZyStatus(id, 2);
                    r.code = AjaxResultCodeEnum.Success;
                    r.message = "";
                    r.data = "";
                    return Json(r);
                }
                else
                {
                    r.code = AjaxResultCodeEnum.Error;
                    r.message = "删除失败！";
                    r.data = "";
                    return Json(r);
                }
            }
        }

        private List<dto_RelateGroup> GetGroupedRelatedUser()
        {
            //查询关注自己的人的各班级数量
            List<dto_RelateGroup> ul = B_User.GetGroupedRelatedUser(UserId);
            if (ul != null)
            {
                string GName = "";
                ul.ForEach(a => {
                    Const.Grades.TryGetValue(a.GradeId, out GName);
                    a.GradeName = GName;
                    a.ClassName = a.ClassId + "班";
                });
            }
            return ul;
        }

        public ActionResult Info(long zyId)
        {
            ViewBag.ZyId = zyId;
            return View();
        }

        public JsonResult GetZyInfo(long zyId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            dto_Zy zy = B_ZyRedis.GetZy(id);
            zy.Id = 0;  //隐藏真实Id
            dto_AjaxJsonResult<dto_Zy> r = new dto_AjaxJsonResult<dto_Zy>();
            if (zy.UserId != UserId)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "这不是您新建的作业，无权查看！";
                r.data = null;
            }
            else
            {
                r.code = AjaxResultCodeEnum.Success;
                r.message = "";
                r.data = zy;
            }
            return Json(r);
        }

        public ActionResult GetQuestions(int courseId, long zyId)
        {
            int id = IdNamingHelper.Decrypt(IdNamingHelper.IdTypeEnum.Zy, zyId);
            List<dto_Question> ql = B_ZyRedis.GetQdbZyQuestions(courseId, id);
            if (ql != null)
            {
                foreach (dto_Question q in ql)
                {
                    //隐藏真实Id
                    q.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, q.id);
                    q.id = 0;
                    if (q.Children != null && q.Children.Count > 0)
                    {
                        q.Children.ForEach(a => {
                            a.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Ques, a.id);
                            a.id = 0;
                        });
                    }
                }
            }
            ViewBag.QuesList = ql;
            return PartialView();
        }
        
    }
}