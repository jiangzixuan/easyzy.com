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

namespace easyzy.common
{
    public static class MySqlDBHelper
    {
        //// 用于缓存参数的HASH表
        //private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());


        ///// <summary>
        /////  给定连接的数据库用假设参数执行一个sql命令（不返回数据集）
        ///// </summary>
        ///// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        ///// <param name="cmdText">存储过程名称或者sql命令语句</param>
        ///// <param name="commandParameters">执行命令所用参数的集合</param>
        ///// <returns>执行命令所影响的行数</returns>
        //public static int ExecuteNonQuery(string connectStringName, string cmdText, CommandType cmdType = CommandType.Text, params MySqlParameter[] commandParameters)
        //{


        //    MySqlCommand cmd = new MySqlCommand();


        //    using (MySqlConnection conn = new MySqlConnection(Util.GetConnectString(connectStringName)))
        //    {
        //        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
        //        int val = cmd.ExecuteNonQuery();
        //        cmd.Parameters.Clear();
        //        return val;
        //    }
        //}


        ///// <summary>
        ///// 用执行的数据库连接执行一个返回数据集的sql命令
        ///// </summary>
        ///// <remarks>
        ///// 举例:
        /////  MySqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        ///// </remarks>
        ///// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        ///// <param name="cmdText">存储过程名称或者sql命令语句</param>
        ///// <param name="commandParameters">执行命令所用参数的集合</param>
        ///// <returns>包含结果的读取器</returns>
        //public static MySqlDataReader ExecuteReader(string connectStringName, string cmdText, params MySqlParameter[] commandParameters)
        //{
        //    //创建一个MySqlCommand对象
        //    MySqlCommand cmd = new MySqlCommand();
        //    //创建一个MySqlConnection对象
        //    MySqlConnection conn = new MySqlConnection(Util.GetConnectString(connectStringName));
        //    CommandType cmdType = CommandType.Text;

        //    //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，
        //    //因此commandBehaviour.CloseConnection 就不会执行
        //    try
        //    {
        //        //调用 PrepareCommand 方法，对 MySqlCommand 对象设置参数
        //        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
        //        //调用 MySqlCommand  的 ExecuteReader 方法
        //        MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //        //清除参数
        //        cmd.Parameters.Clear();
        //        return reader;
        //    }
        //    catch
        //    {
        //        //关闭连接，抛出异常
        //        conn.Close();
        //        throw;
        //    }
        //}


        ///// <summary>
        ///// 返回DataSet
        ///// </summary>
        ///// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        ///// <param name="cmdText">存储过程名称或者sql命令语句</param>
        ///// <param name="commandParameters">执行命令所用参数的集合</param>
        ///// <returns></returns>
        //public static DataSet GetDataSet(string connectStringName, string cmdText, params MySqlParameter[] commandParameters)
        //{
        //    //创建一个MySqlCommand对象
        //    MySqlCommand cmd = new MySqlCommand();
        //    //创建一个MySqlConnection对象
        //    MySqlConnection conn = new MySqlConnection(Util.GetConnectString(connectStringName));
        //    CommandType cmdType = CommandType.Text;

        //    //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，


        //    try
        //    {
        //        //调用 PrepareCommand 方法，对 MySqlCommand 对象设置参数
        //        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
        //        //调用 MySqlCommand  的 ExecuteReader 方法
        //        MySqlDataAdapter adapter = new MySqlDataAdapter();
        //        adapter.SelectCommand = cmd;
        //        DataSet ds = new DataSet();


        //        adapter.Fill(ds);
        //        //清除参数
        //        cmd.Parameters.Clear();
        //        conn.Close();
        //        return ds;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}



        ///// <summary>
        ///// 用指定的数据库连接字符串执行一个命令并返回一个数据集的第一列
        ///// </summary>
        ///// <remarks>
        /////例如:
        /////  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        ///// </remarks>
        ///// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        ///// <param name="cmdText">存储过程名称或者sql命令语句</param>
        ///// <param name="commandParameters">执行命令所用参数的集合</param>
        ///// <returns>用 Convert.To{Type}把类型转换为想要的 </returns>
        //public static object ExecuteScalar(string connectStringName, string cmdText, params MySqlParameter[] commandParameters)
        //{
        //    MySqlCommand cmd = new MySqlCommand();
        //    CommandType cmdType = CommandType.Text;

        //    using (MySqlConnection connection = new MySqlConnection(Util.GetConnectString(connectStringName)))
        //    {
        //        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        //        object val = cmd.ExecuteScalar();
        //        cmd.Parameters.Clear();
        //        return val;
        //    }
        //}


        ///// <summary>
        ///// 将参数集合添加到缓存
        ///// </summary>
        ///// <param name="cacheKey">添加到缓存的变量</param>
        ///// <param name="commandParameters">一个将要添加到缓存的sql参数集合</param>
        //public static void CacheParameters(string cacheKey, params MySqlParameter[] commandParameters)
        //{
        //    parmCache[cacheKey] = commandParameters;
        //}


        ///// <summary>
        ///// 找回缓存参数集合
        ///// </summary>
        ///// <param name="cacheKey">用于找回参数的关键字</param>
        ///// <returns>缓存的参数集合</returns>
        //public static MySqlParameter[] GetCachedParameters(string cacheKey)
        //{
        //    MySqlParameter[] cachedParms = (MySqlParameter[])parmCache[cacheKey];


        //    if (cachedParms == null)
        //        return null;


        //    MySqlParameter[] clonedParms = new MySqlParameter[cachedParms.Length];


        //    for (int i = 0, j = cachedParms.Length; i < j; i++)
        //        clonedParms[i] = (MySqlParameter)((ICloneable)cachedParms[i]).Clone();


        //    return clonedParms;
        //}


        ///// <summary>
        ///// 准备执行一个命令
        ///// </summary>
        ///// <param name="cmd">sql命令</param>
        ///// <param name="conn">OleDb连接</param>
        ///// <param name="trans">OleDb事务</param>
        ///// <param name="cmdType">命令类型例如 存储过程或者文本</param>
        ///// <param name="cmdText">命令文本,例如:Select * from Products</param>
        ///// <param name="cmdParms">执行命令的参数</param>
        //private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        //{
        //    if (conn.State != ConnectionState.Open)
        //        conn.Open();


        //    cmd.Connection = conn;
        //    cmd.CommandText = cmdText;


        //    if (trans != null)
        //        cmd.Transaction = trans;


        //    cmd.CommandType = cmdType;


        //    if (cmdParms != null)
        //    {
        //        foreach (MySqlParameter parameter in cmdParms)
        //        {
        //            if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
        //                (parameter.Value == null))
        //            {
        //                parameter.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(parameter);
        //        }
        //    }
        //}


        #region 有关参数生成的方法---------------------------------------------------

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

        #region
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
