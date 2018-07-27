using easyzy.sdk;
using hw.easyzy.common;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easyzy.sdk.Const;

namespace hw.easyzy.bll
{
    public class B_ZyRedis
    {
        //缓存有效期(7天）
        private static TimeSpan ts = new TimeSpan(7, 0, 0, 0);

        /// <summary>
        /// 获取作业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static dto_Zy GetZy(int id)
        {
            dto_Zy tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Zy, id.ToString());
            using (var client = RedisHelper.Instance.GetRedisClient(CacheCatalog.Zy.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<dto_Zy>(key);
                    if (tempresult == null)
                    {
                        T_Zy z = B_Zy.GetZy(id);
                        string subName = "";
                        Subjects.TryGetValue(z.SubjectId, out subName);
                        tempresult = new dto_Zy()
                        {
                            Id = z.Id,
                            UserId = z.UserId,
                            ZyName = z.ZyName,
                            Type = z.Type,
                            CourseId = z.CourseId,
                            SubjectId = z.SubjectId,
                            CreateDate =z.CreateDate,
                            OpenDate = z.OpenDate,
                            DueDate = z.DueDate,
                            OpenDateStr = z.OpenDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            DueDateStr = z.DueDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            SubjectName = subName,
                            TypeName = z.Type == 0 ? "题库" : "自传",
                            Status = z.Status
                        };

                        if (tempresult != null)
                        {
                            client.Set(key, tempresult, ts);
                        }
                    }
                }
            }
            
            return tempresult;
        }

        /// <summary>
        /// 获取作业的所有试题信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public static List<dto_Question> GetQdbZyQuestions(int courseId, int zyId)
        {
            List<dto_Question> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.QdbZyAllQuestions, zyId.ToString());
            using (var client = RedisHelper.Instance.GetRedisClient(CacheCatalog.QdbZyAllQuestions.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<List<dto_Question>>(key);
                    if (tempresult == null)
                    {
                        string s = GetQdbZyQuesJson(zyId);
                        if (string.IsNullOrEmpty(s)) return null;
                        List<dto_ZyQuestion> zyql = JsonConvert.DeserializeObject<List<dto_ZyQuestion>>(s);
                        if (zyql != null)
                        {
                            tempresult = new List<dto_Question>();
                            zyql.Select(a=>a.PQId).Distinct().ToList().ForEach(b =>
                            {
                                tempresult.Add(B_QuesRedis.GetQuestion(courseId, b));
                            });

                            client.Set(key, tempresult, ts);
                        }
                    }
                }
            }

            return tempresult;
        }

        /// <summary>
        /// 获取作业的试题json信息
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        public static string GetQdbZyQuesJson(int zyId)
        {
            string tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.QdbZyQues, zyId.ToString());
            using (var client = RedisHelper.Instance.GetRedisClient(CacheCatalog.QdbZyQues.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<string>(key);
                    if (tempresult == null)
                    {
                        tempresult = B_Zy.GetQdbZyQues(zyId);

                        if (tempresult != null)
                        {
                            client.Set(key, tempresult, ts);
                        }
                    }
                }
            }

            return tempresult;
        }

        /// <summary>
        /// 获取作业结构信息
        /// </summary>
        /// <param name="zyId"></param>
        /// <returns></returns>
        //public static List<T_ZyStruct> GetZyStruct(int zyId)
        //{
        //    List<T_ZyStruct> tempresult = null;

        //    string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZyStruct, zyId.ToString());
        //    using (var client = RedisHelper.Instance.GetRedisClient(CacheCatalog.SelfZyStruct.ToString()))
        //    {
        //        if (client != null)
        //        {
        //            tempresult = client.Get<List<T_ZyStruct>>(key);
        //            if (tempresult == null)
        //            {
        //                tempresult = B_Zy.GetZyStruct(zyId);
        //                if (tempresult != null && tempresult.Count > 0)
        //                {
        //                    client.Set<List<T_ZyStruct>>(key, tempresult);
        //                }
        //            }
        //        }
        //    }
            
        //    return tempresult;
        //}

        //public static T_Answer GetZyAnswer(int zyId, string trueName)
        //{
        //    T_Answer tempresult = null;
        //    string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZyAnswer, zyId.ToString() + trueName);
        //    using (var client = RedisHelper.Instance.GetRedisClient(CacheCatalog.SelfZyAnswer.ToString()))
        //    {
        //        if (client != null)
        //        {
        //            tempresult = client.Get<T_Answer>(key);
        //            if (tempresult == null)
        //            {
        //                tempresult = B_Zy.GetZyAnswer(zyId, trueName);
        //                if (tempresult != null)
        //                {
        //                    client.Set<T_Answer>(key, tempresult, ts);
        //                }
        //            }
        //        }
        //    }
            
        //    return tempresult;
        //}

        //public static T_Answer GetZyAnswer(int zyId, int studentId)
        //{
        //    T_Answer tempresult = null;
        //    string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZyAnswer, zyId.ToString() + studentId.ToString());
        //    using (var client = RedisHelper.Instance.GetRedisClient(CacheCatalog.SelfZyAnswer.ToString()))
        //    {
        //        if (client != null)
        //        {
        //            tempresult = client.Get<T_Answer>(key);
        //            if (tempresult == null)
        //            {
        //                tempresult = B_Zy.GetZyAnswer(zyId, studentId);
        //                if (tempresult != null)
        //                {
        //                    client.Set<T_Answer>(key, tempresult, ts);
        //                }
        //            }
        //        }
        //    }
            
        //    return tempresult;
        //}

        /// <summary>
        /// 修改作业为已建答题卡
        /// </summary>
        /// <param name="zyId"></param>
        //public static void UpdateZyStructed(int zyId)
        //{
        //    T_Zy tempresult = null;
        //    string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.SelfZy, zyId.ToString());
        //    using (var client = RedisHelper.Instance.GetRedisClient(CacheCatalog.SelfZy.ToString()))
        //    {
        //        if (client != null)
        //        {
        //            tempresult = client.Get<T_Zy>(key);
        //            if (tempresult != null)
        //            {
        //                tempresult.Structed = true;
        //                client.Set<T_Zy>(key, tempresult, ts);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 删除作业缓存
        /// </summary>
        /// <param name="zyId"></param>
        public static void UpdateZyStatus(int zyId, int status)
        {
            dto_Zy tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Zy, zyId.ToString());
            using (var client = RedisHelper.Instance.GetRedisClient(CacheCatalog.Zy.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<dto_Zy>(key);
                    if (tempresult != null)
                    {
                        tempresult.Status = status;
                        client.Set(key, tempresult, ts);
                    }
                }
            }
        }
    }
}
