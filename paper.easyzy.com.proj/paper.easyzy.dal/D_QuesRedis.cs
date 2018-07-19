using easyzy.sdk;
using paper.easyzy.model.dto;
using paper.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static easyzy.sdk.Const;

namespace paper.easyzy.dal
{
    public class D_QuesRedis
    {
        //缓存有效期(50天）
        private static TimeSpan ts = new TimeSpan(500, 0, 0, 0);

        public static dto_Question GetQuestion(int courseId, int qId)
        {
            dto_Question tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Ques, qId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Ques.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<dto_Question>(key);
                    if (tempresult == null)
                    {
                        tempresult = D_Ques.GetWholeQuestion(courseId, qId);
                        if (tempresult != null)
                        {
                            client.Set(key, tempresult, ts);
                        }
                    }
                }
            }

            return tempresult;
        }
    }
}
