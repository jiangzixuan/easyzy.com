using bbs.easyzy.model.dto;
using bbs.easyzy.model.entity;
using easyzy.sdk;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace bbs.easyzy.bll
{
    public class B_Topic
    {
        private static string BBSConnString = "";
        static B_Topic()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.BBS, out BBSConnString);
        }
        public static dto_Topic GetTopic(int Id)
        {
            dto_Topic model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(BBSConnString),
                "select Id, UserId, Invites, Title, TopicContent, TopicText, CreateDate, Good, Hit, ReplyCount, GradeId, SubjectId, Deleted, Blocked, Ip from T_Topic where Id = @Id",
                "@Id".ToInt32InPara(Id)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<dto_Topic>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 获取话题列表
        /// </summary>
        /// <param name="gradeId"></param>
        /// <param name="subjectId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<dto_Topic> GetTopics(int gradeId, int subjectId, int pageIndex, int pageSize, out int totalCount)
        {
            string tcsql = "T_Topic where Deleted = 0 and Blocked = 0 ";
            List<MySqlParameter> pl = new List<MySqlParameter>();
            if (gradeId != 0)
            {
                tcsql += "and GradeId = @GradeId ";
                pl.Add(new MySqlParameter("@GradeId", gradeId));
            }
            if (subjectId != 0)
            {
                tcsql += "and SubjectId = @SubjectId ";
                pl.Add(new MySqlParameter("@SubjectId", subjectId));
            }

            List<dto_Topic> model = null;
            using (MySqlDataReader dr = MySqlDBHelper.GetPageReader(Util.GetConnectString(BBSConnString),
                "Id, UserId, Invites, Title, TopicContent, TopicText, CreateDate, Good, Hit, ReplyCount, GradeId, SubjectId, Ip",
                tcsql,
                "CreateDate desc",
                pageSize,
                pageIndex,
                out totalCount,
                pl.ToArray()))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntityList<dto_Topic>(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 插入话题，返回话题Id
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public static int AddTopic(T_Topic topic)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(BBSConnString),
                "insert into T_Topic(Id, UserId, Invites, Title, TopicContent, TopicText, CreateDate, Good, Hit, ReplyCount, GradeId, SubjectId, Deleted, Blocked, Ip) values (null, @UserId, @Invites, @Title, @TopicContent, @TopicText, @CreateDate, @Good, @Hit, @ReplyCount, @GradeId, @SubjectId, @Deleted, @Blocked, @Ip); select last_insert_id();",
                "@UserId".ToInt32InPara(topic.UserId),
                "@Invites".ToVarCharInPara(topic.Invites),
                "@Title".ToVarCharInPara(topic.Title),
                "@TopicContent".ToVarCharInPara(topic.TopicContent),
                "@TopicText".ToVarCharInPara(topic.TopicText),
                "@CreateDate".ToDateTimeInPara(topic.CreateDate),
                "@Good".ToInt32InPara(topic.Good),
                "@Hit".ToInt32InPara(topic.Hit),
                "@ReplyCount".ToInt32InPara(topic.ReplyCount),
                "@GradeId".ToInt32InPara(topic.GradeId),
                "@SubjectId".ToInt32InPara(topic.SubjectId),
                "@Deleted".ToBitInPara(topic.Deleted),
                "@Blocked".ToBitInPara(topic.Blocked),
                "@Ip".ToVarCharInPara(topic.Ip)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }

        public static List<dto_Reply> GetReplyList(int topicId, int pageSize, int pageIndex, out int totalCount)
        {
            List<dto_Reply> result = null;

            using (var dr = MySqlDBHelper.GetPageReader(Util.GetConnectString(BBSConnString),
                "Id, UserId, TopicId, ReplyContent, CreateDate, Ip, Blocked",
                "T_Reply where TopicId = @TopicId and Deleted = 0",
                "CreateDate",
                pageSize,
                pageIndex,
                out totalCount,
                "@TopicId".ToInt32InPara(topicId)))//查询数据库
            {
                if (dr != null && dr.HasRows)
                {
                    result = MySqlDBHelper.ConvertDataReaderToEntityList<dto_Reply>(dr);
                }
            }

            return result;
        }

        public static dto_Reply GetReplyById(int id)
        {
            dto_Reply result = null;
            string sql = "select Id, UserId, TopicId, ReplyContent, CreateDate from T_Reply where Id = @Id ";
            using (var reader = MySqlHelper.ExecuteReader(Util.GetConnectString(BBSConnString), sql.ToString(), "@Id".ToInt32InPara(id)))//查询数据库
            {
                if (reader != null && reader.HasRows)
                {
                    result = MySqlDBHelper.ConvertDataReaderToEntitySingle<dto_Reply>(reader);
                }
            }
            return result;
        }

        public static int AddReply(T_Reply m)
        {
            string sql = "insert into T_Reply(Id, UserId, TopicId, ReplyContent, CreateDate, Deleted, Blocked, Ip) values(null, @UserId, @TopicId, @ReplyContent, @CreateDate, @Deleted, @Blocked, @Ip) ";
            MySqlParameter[] p = new MySqlParameter[]
            {
                "@UserID".ToInt32InPara(m.UserId),
                "@TopicId".ToInt32InPara(m.TopicId),
                "@ReplyContent".ToVarCharInPara(m.ReplyContent),
                "@CreateDate".ToDateTimeInPara(m.CreateDate),
                "@Deleted".ToBitInPara(m.Deleted),
                "@Blocked".ToBitInPara(m.Blocked),
                "@Ip".ToVarCharInPara(m.Ip)
            };
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(BBSConnString), sql, p);
            return i;
        }

        public static bool AddTopicReplyCount(int topicId)
        {
            string sql = "update T_Topic set ReplyCount = ReplyCount + 1 where Id = @TopicId";
            MySqlParameter[] p = new MySqlParameter[]
            {
                "@TopicId".ToInt32InPara(topicId)
            };
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(BBSConnString), sql, p);
            return i > 0 ? true : false;
        }

        public static bool AddTopicGoodCount(int topicId, int userId)
        {
            if (AddTopicGood(topicId, userId))
            {
                string sql = "update T_Topic set Good = Good + 1 where Id = @TopicId";
                MySqlParameter[] p = new MySqlParameter[]
                {
                "@TopicId".ToInt32InPara(topicId)
                };
                int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(BBSConnString), sql, p);
                return i > 0;
            }
            else
            {
                return false;
            }
        }

        private static bool AddTopicGood(int topicId, int userId)
        {
            string sql = "insert into T_TopicGood(TopicId, UserId, CreateDate) values(@TopicId, @UserId, @CreateDate) ";
            MySqlParameter[] p = new MySqlParameter[]
            {
                "@TopicId".ToInt32InPara(topicId),
                "@UserId".ToInt32InPara(userId),
                "@CreateDate".ToDateTimeInPara(DateTime.Now)
            };
            int i = MySqlHelper.ExecuteNonQuery(Util.GetConnectString(BBSConnString), sql, p);
            return i > 0;
        }

        public static bool HasTopicSetGood(int topicId, int userId)
        {
            string sql = "select 1 from T_TopicGood where TopicId = @TopicId and UserId = @UserId ";
            var obj = MySqlHelper.ExecuteScalar(Util.GetConnectString(BBSConnString), sql, "@TopicId".ToInt32InPara(topicId), "@UserId".ToInt32InPara(userId));

            return int.Parse(obj.ToString()) > 0;
        }
    }
}
