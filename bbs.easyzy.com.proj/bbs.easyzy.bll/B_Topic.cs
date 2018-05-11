using bbs.easyzy.model.entity;
using easyzy.sdk;
using MySql.Data.MySqlClient;

namespace bbs.easyzy.bll
{
    public class B_Topic
    {
        private static string BBSConnString = "";
        static B_Topic()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.BBS, out BBSConnString);
        }
        public static T_Topic GetTopic(int Id)
        {
            T_Topic model = null;
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(Util.GetConnectString(BBSConnString),
                "select Id, UserId, Invites, Title, TopicContent, CreateDate, Good, Hit, ReplyCount, GradeId, SubjectId, Deleted, Blocked from T_Topic where Id = @Id",
                "@Id".ToInt32InPara(Id)))
            {
                if (dr != null && dr.HasRows)
                {
                    model = MySqlDBHelper.ConvertDataReaderToEntitySingle<T_Topic>(dr);
                }
            }
            return model;
        }

        public static int AddTopic(T_Topic topic)
        {
            object o = MySqlHelper.ExecuteScalar(Util.GetConnectString(BBSConnString),
                "insert into T_Topic(Id, UserId, Invites, Title, TopicContent, CreateDate, Good, Hit, ReplyCount, GradeId, SubjectId, Deleted, Blocked) values (null, @UserId, @Invites, @Title, @TopicContent, @CreateDate, @Good, @Hit, @ReplyCount, @GradeId, @SubjectId, @Deleted, @Blocked); select last_insert_id();",
                "@UserId".ToInt32InPara(topic.UserId),
                "@Invites".ToVarCharInPara(topic.Invites),
                "@Title".ToVarCharInPara(topic.Title),
                "@TopicContent".ToVarCharInPara(topic.TopicContent),
                "@CreateDate".ToDateTimeInPara(topic.CreateDate),
                "@Good".ToInt32InPara(topic.Good),
                "@Hit".ToInt32InPara(topic.Hit),
                "@ReplyCount".ToInt32InPara(topic.ReplyCount),
                "@GradeId".ToInt32InPara(topic.GradeId),
                "@SubjectId".ToInt32InPara(topic.SubjectId),
                "@Deleted".ToBitInPara(topic.Deleted),
                "@Blocked".ToBitInPara(topic.Blocked)
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }
    }
}
