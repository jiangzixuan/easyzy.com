﻿using easyzy.sdk;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace hw.easyzy.bll
{
    /// <summary>
    /// 试题相关基础数据（知识点、章节目录等）
    /// 缓存读比较慢，可能原因是大量数据序列化/服务器配置低
    /// 改由读json文件
    /// </summary>
    public class B_QuesBase
    {
        private static string QuesConnString = "";
        /// <summary>
        /// 所有教材版本
        /// </summary>
        private static List<T_TextBookVersions> allTextBookVersions = null;

        /// <summary>
        /// 课本列表
        /// </summary>
        private static List<T_TextBooks> allTextBooks = null;

        /// <summary>
        /// 相似集
        /// </summary>
        private static List<dto_SimilarCatalogs> allSimilarCatalogs = null;

        /// <summary>
        /// 教材版本json文件路径
        /// </summary>
        private static readonly string _textbookversionsfilepath = "/QuesBaseJson/textbookversions.json";

        /// <summary>
        /// 教材json文件路径
        /// </summary>
        private static readonly string _textbookfilepath = "/QuesBaseJson/textbooks.json";

        /// <summary>
        /// 相似集json文件路径
        /// </summary>
        private static readonly string _similarcatalogsfilepath = "/QuesBaseJson/similarcatalogs.json";

        static B_QuesBase()
        {
            Const.DBConnStrNameDic.TryGetValue(Const.DBName.Ques, out QuesConnString);

            allTextBookVersions = GetJosnFile<List<T_TextBookVersions>>(GetMapPath(_textbookversionsfilepath));

            allTextBooks = GetJosnFile<List<T_TextBooks>>(GetMapPath(_textbookfilepath));

            allSimilarCatalogs = GetJosnFile<List<dto_SimilarCatalogs>>(GetMapPath(_similarcatalogsfilepath));
        }

        public static List<T_TextBookVersions> GetTextBookVersions(int courseId)
        {
            int[] varray = allTextBooks.FindAll(p => p.CourseId.Equals(courseId)).Select(p => p.VersionId).Distinct().ToArray();
            return allTextBookVersions.FindAll(p => varray.Contains(p.Id));
        }

        /// <summary>
        /// 获取版本下的课本
        /// </summary>
        /// <param name="versionId">教材ID</param>
        /// <returns></returns>
        public static List<T_TextBooks> GetTextBooks(int versionId)
        {
            return allTextBooks.FindAll(p => p.VersionId.Equals(versionId));
        }

        public static List<dto_ZTree> GetCatalogsTree(int bookId)
        {
            List<T_CatalogNodes> cataloglist = B_QuesBaseRedis.GetCatalogs(bookId);
            List<dto_ZTree> tree = new List<dto_ZTree>();
            cataloglist.ForEach(p =>
            {
                dto_ZTree node = new dto_ZTree();
                node.id = p.Id;
                node.pId = p.ParentId;
                node.name = p.Name;
                node.ntype = p.Type;
                //node.iconSkin = "icon01";
                tree.Add(node);
            });
            return tree;
        }

        public static List<dto_ZTree> GetKnowledgePointsTree(int courseId)
        {
            List<T_KnowledgePoints> kplist = B_QuesBaseRedis.GetKnowledgePoints(courseId);
            List<dto_ZTree> tree = new List<dto_ZTree>();
            kplist.ForEach(p =>
            {
                dto_ZTree node = new dto_ZTree();
                node.id = p.Id;
                node.pId = p.ParentId;
                node.name = p.Name;
                node.ntype = p.Type;
                //node.iconSkin = "icon01";
                tree.Add(node);
            });
            return tree;
        }

        private static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
        }

        private static T GetJosnFile<T>(string filePath) where T : new()
        {
            T contentObject = default(T);
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                contentObject = JsonConvert.DeserializeObject<T>(content);
            }
            if (contentObject == null)
                contentObject = new T();
            return contentObject;
        }
    }
}
