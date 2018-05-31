﻿using easyzy.sdk;
using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static easyzy.sdk.Const;

namespace hw.easyzy.bll
{
    public class B_QuesRedis
    {
        //缓存有效期(50天）
        private static TimeSpan ts = new TimeSpan(500, 0, 0, 0);
        public static List<T_CatalogNodes> GetCatalogs(int textbookId)
        {
            List<T_CatalogNodes> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Catalogs, textbookId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.Catalogs.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<List<T_CatalogNodes>>(key);
                    if (tempresult == null)
                    {
                        tempresult = B_Ques.GetCatalogNodes(textbookId);
                        if (tempresult != null && tempresult.Count > 0)
                        {
                            client.Set<List<T_CatalogNodes>>(key, tempresult, ts);
                        }
                    }
                }
            }

            return tempresult;
        }

        public static List<T_KnowledgePoints> GetKnowledgePoints(int courseId)
        {
            List<T_KnowledgePoints> tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.KnowledgePoints, courseId.ToString());
            using (var client = RedisHelper.GetRedisClient(CacheCatalog.KnowledgePoints.ToString()))
            {
                if (client != null)
                {
                    Stopwatch sw1 = new Stopwatch();
                    sw1.Start();
                    tempresult = client.Get<List<T_KnowledgePoints>>(key);
                    sw1.Stop();
                    LogHelper.Error(sw1.Elapsed.ToString());
                    if (tempresult == null)
                    {
                        tempresult = B_Ques.GetKnowledgePoints(courseId);
                        if (tempresult != null && tempresult.Count > 0)
                        {
                            client.Set<List<T_KnowledgePoints>>(key, tempresult, ts);
                        }
                    }
                }
            }

            return tempresult;
        }

    }
}