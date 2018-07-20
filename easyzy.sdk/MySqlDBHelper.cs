using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;

namespace easyzy.sdk
{
    /// <summary>
    /// 补充mysql.data的mysql数据库访问帮助类
    /// </summary>
    public static class MySqlDBHelper
    {

        #region 关于分页的方法

        ///<summary>获取分页的Datareader对象
        /// </summary>
        /// <param name="connString">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="fieldList">查询的字段列表，不含“SELECT”</param>
        /// <param name="tableAndCondition">查询的表和条件，不含“FROM”</param>
        /// <param name="orderWay">排序方式，不含“ORDER BY”</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">需要显示第几页</param>
        /// <param name="rsCount">返回得到总记录条数，如果出错返回-1 </param>
        /// <param name="paras">参数列表</param>
        /// <returns>DbDataReader对象，如果出错返回null</returns>
        public static MySqlDataReader GetPageReader(string connString, string fieldList, string tableAndCondition,
            string orderWay, int pageSize, int pageIndex, out int rsCount, params MySqlParameter[] paras)
        {
            MySqlDataReader myDr = MySqlHelper.ExecuteReader(connString, getPageSql(fieldList, tableAndCondition, orderWay, pageSize, pageIndex, true), paras);
            if (myDr != null)
            {
                myDr.Read();
                rsCount = int.Parse(myDr[0].ToString());
                myDr.NextResult();
                return myDr;
            }
            else
            {
                rsCount = -1;
                return null;
            }

        }

        ///<summary>获取分页数据的Datareader对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="fieldList">查询的字段列表，不含“SELECT”</param>
        /// <param name="tableAndCondition">查询的表和条件，不含“FROM”</param>
        /// <param name="orderWay">排序方式，不含“ORDER BY”</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">需要显示第几页</param>
        /// <param name="paras">参数列表</param>
        /// <returns>DbDataReader对象，如果出错返回null</returns>
        public static MySqlDataReader GetPageReader(string connString, string fieldList, string tableAndCondition,
            string orderWay, int pageSize, int pageIndex, params MySqlParameter[] paras)
        {
            return MySqlHelper.ExecuteReader(connString, getPageSql(fieldList, tableAndCondition, orderWay, pageSize, pageIndex, false), paras);
        }

        ///<summary>获取分页数据的Table对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="fieldList">查询的字段列表，不含“SELECT”</param>
        /// <param name="tableAndCondition">查询的表和条件，不含“FROM”</param>
        /// <param name="orderWay">排序方式，不含“ORDER BY”</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">需要显示第几页</param>
        /// <param name="paras">参数列表</param>
        /// <param name="rsCount">返回得到总记录条数,如果出错返回-1</param>
        /// <returns>Table对象，如果出错返回null</returns>
        public static DataTable GetPageTable(string connString, string fieldList, string tableAndCondition,
            string orderWay, int pageSize, int pageIndex, out int rsCount, params MySqlParameter[] paras)
        {
            DataSet rt = MySqlHelper.ExecuteDataset(connString, getPageSql(fieldList, tableAndCondition, orderWay, pageSize, pageIndex, true), paras);
            if (rt != null)
            {
                rsCount = int.Parse(rt.Tables[0].Rows[0][0].ToString());
                return rt.Tables[1];
            }
            else
            {
                rsCount = -1;
                return null;
            }
        }

        /// <summary>
        /// 组装分页查询的SQL语句，不兼容distinct
        /// </summary>
        /// <param name="fieldList">查询的字段列表，不含“SELECT”</param>
        /// <param name="tableAndCondition">查询的表和条件，不含“FROM”</param>
        /// <param name="orderWay">排序方式，不含“ORDER BY”</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">需要显示第几页</param>
        /// <param name="neekct">是否需要计算总数</param>
        /// <returns>返回SQL语句</returns>
        private static string getPageSql(string fieldList, string tableAndCondition, string orderWay, int pageSize, int pageIndex, bool needct)
        {
            int begin = (pageIndex - 1) * pageSize;
            if (begin < 0) begin = 0;
            StringBuilder tmpSql = new StringBuilder(string.Empty);
            if (needct)
            {
                tmpSql.Append("select count(1) from " + tableAndCondition + ";");
            }

            tmpSql.Append("select " + fieldList + " from " + tableAndCondition + " order by " + orderWay + " limit " + begin + "," + pageSize);
            return tmpSql.ToString();
        }

        /// <summary>
        /// 兼容distinct
        /// </summary>
        /// <param name="fieldList"></param>
        /// <param name="tableAndCondition"></param>
        /// <param name="orderWay"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="needct"></param>
        /// <returns></returns>
        private static string getPageSql2(string fieldList, string tableAndCondition, string orderWay, int pageSize, int pageIndex, bool needct)
        {
            int begin = (pageIndex - 1) * pageSize;
            if (begin < 0) begin = 0;
            StringBuilder tmpSql = new StringBuilder(string.Empty);
            if (needct)  //需要计算总数，在外面套一层是为了兼容distinct
            {
                tmpSql.Append("select count(1) from (select " + fieldList + " from " + tableAndCondition + ") s;");
            }

            tmpSql.Append("select " + fieldList + " from " + tableAndCondition + " order by " + orderWay + " limit " + begin + "," + pageSize);
            return tmpSql.ToString();
        }

        #endregion

        #region 关于Mysql参数生成的方法---------------------------------------------------

        /// <summary>生成存储过程输入参数（varchar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToVarCharInPara(this string paraName, string value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.VarChar;
            return rt;
        }

        /// <summary>生成存储过程输入参数（int32）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToInt32InPara(this string paraName, int value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.Int32;
            return rt;
        }

        /// <summary>生成存储过程输入参数（Datetime）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToDateTimeInPara(this string paraName, DateTime value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.DateTime;
            return rt;
        }

        /// <summary>生成存储过程输入参数（Datetime）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToBitInPara(this string paraName, bool value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.Bit;
            return rt;
        }

        /// <summary>生成存储过程输入参数（Text）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToTextInPara(this string paraName, string value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.Text;
            return rt;
        }

        /// <summary>生成存储过程输入参数（Float）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToFloatInPara(this string paraName, float value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.Float;
            return rt;
        }

        public static MySqlParameter ToDecimalInPara(this string paraName, decimal value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.Decimal;
            return rt;
        }

        /// <summary>生成存储过程输出参数（varchar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="size">参数的长度</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToVarCharOutPara(this string paraName, int size)
        {
            MySqlParameter rt = new MySqlParameter();
            rt.MySqlDbType = MySqlDbType.VarChar;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            rt.Size = size;
            return rt;
        }

        /// <summary>生成存储过程输出参数（Int32）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToInt32OutPara(this string paraName)
        {
            MySqlParameter rt = new MySqlParameter();
            rt.MySqlDbType = MySqlDbType.Int32;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            return rt;

        }

        /// <summary>生成存储过程输出参数（Float）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToFloatOutPara(this string paraName)
        {
            MySqlParameter rt = new MySqlParameter();
            rt.MySqlDbType = MySqlDbType.Float;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            return rt;
        }

        /// <summary>生成存储过程输出参数（DateTime）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToDateTimeOutPara(this string paraName)
        {
            MySqlParameter rt = new MySqlParameter();
            rt.MySqlDbType = MySqlDbType.DateTime;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            return rt;
        }

        /// <summary>生成存储过程输出参数（Text）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToTextOutPara(this string paraName)
        {
            MySqlParameter rt = new MySqlParameter();
            rt.MySqlDbType = MySqlDbType.Text;
            rt.Direction = ParameterDirection.Output;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（VarChar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <param name="size">参数的长度</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToVarCharInOutPara(this string paraName, string value, int size)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.VarChar;
            rt.Direction = ParameterDirection.InputOutput;
            rt.Size = size;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（Int32）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToInt32InOutPara(this string paraName, int value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.Int32;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（float）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToFloatInOutPara(this string paraName, float value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.Float;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（DateTime）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToDateTimeInOutPara(this string paraName, DateTime value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.DateTime;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（Text）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>MySqlParameter</returns>
        public static MySqlParameter ToTextInOutPara(this string paraName, string value)
        {
            MySqlParameter rt = new MySqlParameter(paraName, value);
            rt.MySqlDbType = MySqlDbType.Text;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }
        #endregion

        #region 关于DataReader/DataTable转List/Single的方法
        public static List<T> ConvertDataTableToEntityList<T>(DataTable dt) where T : new()
        {
            var type = typeof(T);
            var list = new List<T>();
            if (dt.Rows.Count == 0)
            {
                return list;
            }
            var pros = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();
                foreach (var p in pros)
                {
                    if (p.CanWrite)
                    {
                        if (dt.Columns.Contains(p.Name) && !Convert.IsDBNull(dr[p.Name]))
                        {
                            p.SetValue(t, dr[p.Name], null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;

        }
        /// <summary>
        /// 将数据表转换为实体类。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public static List<T> ConvertDataReaderToEntityList<T>(MySqlDataReader dr) where T : new()
        {
            List<T> list = new List<T>();
            while (dr.Read())
            {
                T t = System.Activator.CreateInstance<T>();
                Type obj = t.GetType();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    object tempValue = null;
                    if (dr.IsDBNull(i))
                    {
                        if (obj.GetProperty(dr.GetName(i)) != null)
                        {
                            string typeFullName = obj.GetProperty(dr.GetName(i)).PropertyType.FullName;
                            tempValue = GetDBNullValue(typeFullName);
                        }
                    }
                    else
                    {
                        tempValue = dr.GetValue(i);
                    }
                    if (obj.GetProperty(dr.GetName(i)) != null)
                    {
                        obj.GetProperty(dr.GetName(i)).SetValue(t, tempValue, null);
                    }

                }
                list.Add(t);
            }
            return list;
            
        }

        /// <summary>
        /// 将数据某条记录转换为实体类。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">数据某条记录</param>
        /// <returns></returns>
        public static T ConvertDataTableToEntitySingle<T>(DataTable dt) where T : new()
        {
            if (dt.Rows.Count == 0)
            {
                return default(T);
            }

            var type = typeof(T);
            var result = new T();
            var pros = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();
                foreach (var p in pros)
                {
                    if (p.CanWrite)
                    {
                        if (dt.Columns.Contains(p.Name) && !Convert.IsDBNull(dr[p.Name]))
                        {
                            p.SetValue(t, dr[p.Name], null);
                        }
                    }
                }

                result = t;
            }
            return result;
        }

        /// <summary>
        /// 将数据某条记录转换为实体类。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">数据某条记录</param>
        /// <returns></returns>
        public static T ConvertDataReaderToEntitySingle<T>(MySqlDataReader dr) where T : new()
        {

            if (!dr.HasRows)
            {
                return default(T);
            }
            var type = typeof(T);
            var result = new T();
            var pros = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (dr.Read())
            {
                var t = new T();
                foreach (var p in pros)
                {
                    try
                    {
                        //dr.GetOrdinal(p.Name)如果字段不存在会抛异常
                        if (p.CanWrite)
                        {
                            if (dr.GetOrdinal(p.Name) != -1 && !Convert.IsDBNull(dr[p.Name]))
                            {
                                p.SetValue(t, dr[p.Name], null);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                result = t;
            }
            return result;
        }

        /// <summary>  
        /// 返回值为DBnull的默认值  
        /// </summary>  
        /// <param name="typeFullName">数据类型的全称，类如：system.int32</param>  
        /// <returns>返回的默认值</returns>  
        private static object GetDBNullValue(string typeFullName)
        {
            //typeFullName = typeFullName.ToLower();

            if (typeFullName == "System.String")
            {
                return String.Empty;
            }

            if (typeFullName == "System.Int32")
            {
                return 0;
            }
            if (typeFullName == "System.DateTime")
            {
                return DateTime.MinValue;
            }
            if (typeFullName == "System.Boolean")
            {
                return false;
            }
            return null;
        }
        #endregion
    }
}
