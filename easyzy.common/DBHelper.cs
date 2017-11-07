using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Concurrent;

namespace easyzy.common
{
    public static class DBHelper
    {
        //static ILog log;

        static ConcurrentDictionary<Type, Delegate> ExpressionCache = new ConcurrentDictionary<Type, Delegate>();

        static DBHelper()
        {

        }

        #region 私有方法------------------------------------------------------------

        /// <summary>保存数据操作的出错记录
        /// </summary>
        /// <param name="db">数据库id</param>
        /// <param name="cmdType">命令类型（0 sql不带参数  1 sql带参数）</param>
        /// <param name="cmdStr">命令字符串</param>
        /// <param name="errMsg">错误信息内容</param>
        /// <param name="paras">参数列表</param>
        private static void saveProcError(string db, int cmdType, string cmdStr, string errMsg, params SqlParameter[] paras)
        {
            StringBuilder outStr = new StringBuilder();
            outStr.Append("{\"time\":\"");
            outStr.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            outStr.Append("\",\"db\":\"");
            outStr.Append(db);
            outStr.Append("\",\"cmdType\":");
            outStr.Append(cmdType.ToString());
            outStr.Append(",\"cmdStr\":\"");
            outStr.Append(cmdStr);
            outStr.Append("\",err:\"");
            outStr.Append(errMsg.Replace("\"", "\\\""));
            outStr.Append("\"");
            if (paras != null)
            {
                outStr.Append(",\"para\":[");
                for (int i = 0; i < paras.Length; i += 2)
                {
                    outStr.Append("\"");
                    outStr.Append(paras[i].ParameterName);  //参数名称，没有特殊字符，无需做替换处理
                    outStr.Append("\",\"");
                    outStr.Append(paras[i].Value.ToString().Replace("\"", "\\\""));
                    outStr.Append("\",\"");
                    outStr.Append(paras[i].Direction == ParameterDirection.Input ? "I\"" : "O\"");
                }
                outStr.Append("]");

            }
            outStr.Append("}\r\n");
            LogHelper.Error(outStr.ToString());
        }

        /// <summary>组装分页查询的SQL语句
        /// </summary>
        /// <param name="fieldList">查询的字段列表，不含“SELECT”</param>
        /// <param name="tableAndCondition">查询的表和条件，不含“FROM”</param>
        /// <param name="orderWay">排序方式，不含“ORDER BY”</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">需要显示第几页</param>
        /// <param name="neekct">是否需要计算总数</param>
        /// <returns>返回SQL语句</returns>
        private static string getPageSql(string fieldList, string tableAndCondition, string orderWay, int pageSize, int pageIndex, bool neekct)
        {
            int begin = (pageIndex - 1) * pageSize;
            if (begin < 0) begin = 0;
            StringBuilder tmpSql = new StringBuilder(string.Empty);
            if (neekct)  //需要计算总数
            {
                int _pos = tableAndCondition.IndexOf(" group ", StringComparison.OrdinalIgnoreCase);
                if (_pos > -1)
                {
                    tmpSql.Append("WITH tb0 AS (SELECT ");
                    tmpSql.Append(tableAndCondition.Substring(_pos + 9));
                    tmpSql.Append(" FROM ");
                    tmpSql.Append(tableAndCondition);
                    tmpSql.Append(") SELECT COUNT(*) FROM tb0;");
                }
                else
                {
                    tmpSql.Append("SELECT count(*) FROM ");
                    tmpSql.Append(tableAndCondition);
                    tmpSql.Append(";");
                }
            }
            tmpSql.Append("WITH tb1 AS (SELECT ");

            tmpSql.Append(fieldList);
            tmpSql.Append(",ROW_NUMBER() OVER( ORDER BY ");
            tmpSql.Append(orderWay);
            tmpSql.Append(") AS __ord");
            tmpSql.Append(" FROM ");
            tmpSql.Append(tableAndCondition);
            tmpSql.Append(" ) SELECT TOP ");
            tmpSql.Append(pageSize.ToString());
            tmpSql.Append(" ");
            tmpSql.Append("*");
            tmpSql.Append(" FROM tb1 WHERE __ord>");
            tmpSql.Append(begin.ToString());
            tmpSql.Append(" order by __ord");
            return tmpSql.ToString();
        }

        #endregion

        #region 有关参数生成的方法---------------------------------------------------


        /// <summary>生成存储过程输入参数（char）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToCharInPara(this string paraName, string value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Char;
            return rt;
        }

        /// <summary>生成存储过程输入参数（varchar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToVarCharInPara(this string paraName, string value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.VarChar;
            return rt;
        }

        /// <summary>生成存储过程输入参数（int）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToIntInPara(this string paraName, int value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Int;
            return rt;
        }

        /// <summary>生成存储过程输入参数（Datetime）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToDateTimeInPara(this string paraName, DateTime value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.DateTime;
            return rt;
        }

        /// <summary>生成存储过程输入参数（Datetime）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToBitInPara(this string paraName, bool value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Bit;
            return rt;
        }

        /// <summary>生成存储过程输入参数（Text）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToTextInPara(this string paraName, string value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Text;
            return rt;
        }

        /// <summary>生成存储过程输入参数（NText）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToNTextInPara(this string paraName, string value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.NText;
            return rt;
        }

        /// <summary>生成存储过程输入参数（NChar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToNCharInPara(this string paraName, string value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.NChar;
            return rt;
        }

        /// <summary>生成存储过程输入参数（NVarChar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToNVarCharInPara(this string paraName, string value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.NVarChar;
            return rt;
        }

        /// <summary>生成存储过程输入参数（Float）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToFloatInPara(this string paraName, float value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Float;
            return rt;
        }

        public static SqlParameter ToDecimalInPara(this string paraName, decimal value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Decimal;
            return rt;
        }

        /// <summary>生成存储过程输入参数（Image，二进制流）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToImageInPara(this string paraName, byte[] value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Image;
            return rt;
        }

        /// <summary>生成存储过程输出参数（char）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="size">参数的长度</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToCharOutPara(this string paraName, int size)
        {
            SqlParameter rt = new SqlParameter();
            rt.SqlDbType = SqlDbType.Char;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            rt.Size = size;
            return rt;
        }

        /// <summary>生成存储过程输出参数（varchar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="size">参数的长度</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToVarCharOutPara(this string paraName, int size)
        {
            SqlParameter rt = new SqlParameter();
            rt.SqlDbType = SqlDbType.VarChar;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            rt.Size = size;
            return rt;
        }

        /// <summary>生成存储过程输出参数（int）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToIntOutPara(this string paraName)
        {
            SqlParameter rt = new SqlParameter();
            rt.SqlDbType = SqlDbType.Int;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            return rt;

        }

        /// <summary>生成存储过程输出参数（Float）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToFloatOutPara(this string paraName)
        {
            SqlParameter rt = new SqlParameter();
            rt.SqlDbType = SqlDbType.Float;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            return rt;
        }

        /// <summary>生成存储过程输出参数（DateTime）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToDateTimeOutPara(this string paraName)
        {
            SqlParameter rt = new SqlParameter();
            rt.SqlDbType = SqlDbType.DateTime;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            return rt;
        }

        /// <summary>生成存储过程输出参数（Text）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToTextOutPara(this string paraName)
        {
            SqlParameter rt = new SqlParameter();
            rt.SqlDbType = SqlDbType.Text;
            rt.Direction = ParameterDirection.Output;
            return rt;
        }

        /// <summary>生成存储过程输出参数（NText）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToNTextOutPara(this string paraName)
        {
            SqlParameter rt = new SqlParameter();
            rt.ParameterName = paraName;
            rt.SqlDbType = SqlDbType.NText;
            rt.Direction = ParameterDirection.Output;
            return rt;
        }

        /// <summary>生成存储过程输出参数（NChar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="size">参数的长度</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToNCharOutPara(this string paraName, int size)
        {
            SqlParameter rt = new SqlParameter();
            rt.SqlDbType = SqlDbType.NChar;
            rt.ParameterName = paraName;
            rt.Size = size;
            rt.Direction = ParameterDirection.Output;
            return rt;
        }

        /// <summary>生成存储过程输出参数（NVarChar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="size">参数的长度</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToNVarCharOutPara(this string paraName, int size)
        {
            SqlParameter rt = new SqlParameter();
            rt.SqlDbType = SqlDbType.NVarChar;
            rt.ParameterName = paraName;
            rt.Direction = ParameterDirection.Output;
            rt.Size = size;
            return rt;
        }

        /// <summary>生成存储过程输出参数（Image,二进制数组）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToImageOutPara(this string paraName)
        {
            SqlParameter rt = new SqlParameter();
            rt.SqlDbType = SqlDbType.Image;
            rt.Direction = ParameterDirection.Output;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（Char）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <param name="size">参数的长度</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToCharInOutPara(this string paraName, string value, int size)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Char;
            rt.Direction = ParameterDirection.InputOutput;
            rt.Size = size;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（VarChar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <param name="size">参数的长度</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToVarCharInOutPara(this string paraName, string value, int size)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.VarChar;
            rt.Direction = ParameterDirection.InputOutput;
            rt.Size = size;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（NChar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <param name="size">参数的长度</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToNCharInOutPara(this string paraName, string value, int size)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.NChar;
            rt.Direction = ParameterDirection.InputOutput;
            rt.Size = size;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（NVarChar）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <param name="size">参数的长度</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToNVarCharInOutPara(this string paraName, string value, int size)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.NVarChar;
            rt.Direction = ParameterDirection.InputOutput;
            rt.Size = size;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（int）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToIntInOutPara(this string paraName, int value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Int;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（float）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToFloatInOutPara(this string paraName, float value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Float;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（DateTime）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToDateTimeInOutPara(this string paraName, DateTime value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.DateTime;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（Text）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToTextInOutPara(this string paraName, string value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Text;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（NText）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToNTextInOutPara(this string paraName, string value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.NText;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }

        /// <summary>生成存储过程输入输出参数（Image,二进制数组）
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">输入的参数值</param>
        /// <returns>sqlParameter</returns>
        public static SqlParameter ToImageInOutPara(this string paraName, byte[] value)
        {
            SqlParameter rt = new SqlParameter(paraName, value);
            rt.SqlDbType = SqlDbType.Image;
            rt.Direction = ParameterDirection.InputOutput;
            return rt;
        }

        #endregion

        #region 公共方法------------------------------------------------------------

        /// <summary>设置日志打印
        /// </summary>
        /// <param name="logger">log4net的实例</param>
        //public static void Config(ILog logger)
        //{
        //    log = logger;
        //}



        /// <summary>执行SQL语句并返回影响的行数
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="sqlStr">要执行的查询SQL语句</param>
        /// <param name="paras">可选参数数组</param>
        /// <returns>执行SQL语句影响行数，执行失败返回 -1 </returns>
        public static int ExcuteSQL(string connKey, string sqlStr, params SqlParameter[] paras)
        {
            int result = 0; //-1;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString))
            {

                SqlCommand comd = new SqlCommand(sqlStr, conn);
                comd.CommandType = CommandType.Text;
                if (paras != null)
                    comd.Parameters.AddRange(paras);
                try
                {
                    conn.Open();
                    result = comd.ExecuteNonQuery();

                }
                catch (SqlException err)
                {
                    saveProcError(connKey, paras == null ? 0 : 1, sqlStr, err.Message + err.StackTrace, paras);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        /// <summary> 获取数据集对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="sqlStr">要执行的查询SQL语句</param>
        /// <param name="paras">可选参数数组</param>
        /// <returns>得到的数据集，发生错误则返回 null</returns>
        public static DataSet GetDataSet(string connKey, string sqlStr, params SqlParameter[] paras)
        {
            DataSet myDs = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString))
            {
                try
                {
                    SqlCommand myCm = new SqlCommand(sqlStr, conn);
                    myCm.CommandType = CommandType.Text;
                    if (paras != null)
                        myCm.Parameters.AddRange(paras);
                    SqlDataAdapter myDa = new SqlDataAdapter(myCm);

                    myDs = new DataSet();
                    myDa.Fill(myDs);
                }
                catch (SqlException Err)
                {

                    saveProcError(connKey, paras == null ? 0 : 1, sqlStr, Err.Message, paras);
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
            return myDs;
        }

        /// <summary>获取数据表对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="sqlStr">要执行的查询SQL语句</param>
        /// <param name="paras">可选参数数组</param>
        /// <returns>得到的数据表，发生错误则返回 null</returns>
        public static DataTable GetDataTable(string connKey, string sqlStr, params SqlParameter[] paras)
        {
            DataSet myDs = GetDataSet(connKey, sqlStr, paras);
            if (myDs != null)
                return myDs.Tables[0];
            else
                return null;
        }

        /// <summary>获取数据表对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="sqlStr">要执行的查询SQL语句</param>
        /// <param name="myDa">需要返回的中间桥接器</param>
        /// <param name="paras">可选参数数组</param>
        /// <returns>得到的数据表，发生错误则返回 null</returns>
        public static DataTable GetDataTableByAdpater(string connKey, string sqlStr, ref SqlDataAdapter myDa, params SqlParameter[] paras)
        {

            DataTable myDt = null;
            SqlConnection myCn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString);

            SqlCommand myCm = new SqlCommand(sqlStr, myCn);
            myCm.CommandType = CommandType.Text;
            if (paras != null)
                myCm.Parameters.AddRange(paras);
            myDa = new SqlDataAdapter(myCm);
            try
            {
                myDt = new DataTable();
                myDa.Fill(myDt);
            }
            catch (SqlException Err)
            {
                myCn.Close();
                saveProcError(connKey, paras == null ? 0 : 1, sqlStr, Err.Message + Err.StackTrace, paras);
            }
            return myDt;
        }

        /// <summary>获取DataReader对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="sqlStr">要执行的查询SQL语句</param>
        /// <param name="paras">可选参数数组</param>
        /// <returns>得到的DataReader，发生错误则返回 null</returns>
        public static SqlDataReader GetDataReader(string connKey, string sqlStr, params SqlParameter[] paras)
        {
            SqlDataReader myDr = null;
            SqlConnection myCn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString);
            SqlCommand myCm = new SqlCommand(sqlStr, myCn);
            myCm.CommandType = CommandType.Text;
            if (paras != null)
                myCm.Parameters.AddRange(paras);
            try
            {
                myCn.Open();
                try
                {
                    myDr = myCm.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (SqlException err)
                {
                    myCn.Close();
                    saveProcError(connKey, paras == null ? 0 : 1, sqlStr, err.Message + err.StackTrace, paras);
                }
            }
            catch (SqlException err)
            {
                saveProcError(connKey, paras == null ? 0 : 1, sqlStr, err.Message + err.StackTrace, paras);

            }

            return myDr;
        }

        /// <summary>获取单个字段值
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="sqlStr">要执行的查询SQL语句</param>
        /// <param name="paras">可选参数数组</param>
        /// <returns>返回查询的字段</returns>
        public static object GetField(string connKey, string sqlStr, params SqlParameter[] paras)
        {
            object rt = null;
            using (SqlConnection myCn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString))
            {
                SqlCommand myCm = new SqlCommand(sqlStr, myCn);
                myCm.CommandType = CommandType.Text;
                if (paras != null)
                    myCm.Parameters.AddRange(paras);
                try
                {
                    myCn.Open();
                    rt = myCm.ExecuteScalar();
                }
                catch (SqlException err)
                {
                    saveProcError(connKey, paras == null ? 0 : 1, sqlStr, err.Message + err.StackTrace, paras);
                }
                finally
                {
                    myCn.Close();
                }
            }
            return rt;
        }

        /// <summary>
        /// 返回长整形主键
        /// </summary>
        /// <param name="connKey"></param>
        /// <param name="sqlStr"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static long ExecuteScalar(string connKey, string sqlStr, params SqlParameter[] paras)
        {
            long rt = -99999999;
            using (SqlConnection myCn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString))
            {
                SqlCommand myCm = new SqlCommand(sqlStr, myCn);
                myCm.CommandType = CommandType.Text;
                if (paras != null)
                    myCm.Parameters.AddRange(paras);
                try
                {
                    myCn.Open();
                    rt = Convert.ToInt64(myCm.ExecuteScalar());
                }
                catch (SqlException err)
                {
                    saveProcError(connKey, paras == null ? 0 : 1, sqlStr, err.Message + err.StackTrace, paras);
                }
                finally
                {
                    myCn.Close();
                }
            }
            return rt;
        }

        /// <summary>执行存储过程得到DataReader对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="stpName">存储过程名称</param>
        /// <param name="stpPara">可选参数数组</param>
        /// <returns>DataReader对象,注：DataReader的NextResult可用的情况下，分别为输出参数列表和返回值</returns>
        public static SqlDataReader ExecStoreProcR(string connKey, string stpName, params SqlParameter[] stpPara)
        {
            SqlDataReader myDr = null;
            SqlConnection myCn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString);
            SqlCommand myCm = new SqlCommand(stpName, myCn);
            myCm.CommandType = CommandType.StoredProcedure;
            if (stpPara != null)
                myCm.Parameters.AddRange(stpPara);
            try
            {
                myCn.Open();
                try
                {

                    myDr = myCm.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception err)
                {
                    saveProcError(connKey, stpPara == null ? 0 : 1, stpName, err.Message + err.StackTrace, stpPara);
                    myCn.Close();
                }
            }
            catch (SqlException err)
            {
                saveProcError(connKey, stpPara == null ? 0 : 1, stpName, err.Message + err.StackTrace, stpPara);
            }

            return myDr;

        }

        /// <summary>执行存储过程返回传出值参数列表，返回值在stpPara参数中获取
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="stpName">存储过程名称</param>
        /// <param name="stpPara">可选参数数组</param>
        public static void ExecStoreProcP(string connKey, string stpName, params SqlParameter[] stpPara)
        {
            using (SqlConnection myCn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString))
            {
                SqlCommand myCm = new SqlCommand(stpName, myCn);
                myCm.CommandType = CommandType.StoredProcedure;
                if (stpPara != null)
                    myCm.Parameters.AddRange(stpPara);
                try
                {
                    myCn.Open();
                    try
                    {
                        myCm.ExecuteNonQuery();
                    }
                    catch (SqlException err)
                    {
                        myCn.Close();
                        saveProcError(connKey, stpPara == null ? 0 : 1, stpName, err.Message + err.StackTrace, stpPara);

                    }

                }
                catch (SqlException err)
                {

                    saveProcError(connKey, stpPara == null ? 0 : 1, stpName, err.Message + err.StackTrace, stpPara);

                }

            }
        }

        /// <summary>执行存储过程得到DataSet对象，并返回传出值参数列表
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="stpName">存储过程名称</param>
        /// <param name="stpPara">存储过程参数列表</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ExecStoreProcS(string connKey, string stpName, params SqlParameter[] stpPara)
        {
            DataSet rt = null;
            try
            {
                using (SqlConnection myCn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString))
                {
                    SqlCommand myCm = new SqlCommand(stpName, myCn);
                    myCm.CommandType = CommandType.StoredProcedure;
                    if (stpPara != null)
                        myCm.Parameters.AddRange(stpPara);
                    SqlDataAdapter myDa = new SqlDataAdapter(myCm);
                    rt = new DataSet();

                    myDa.Fill(rt);
                }

            }
            catch (SqlException err)
            {
                saveProcError(connKey, stpPara == null ? 0 : 1, stpName, err.Message, stpPara);
            }
            return rt;
        }

        /// <summary>执行存储过程得到DataTable对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="stpName">存储过程名称</param>
        /// <param name="stpPara">存储过程参数列表</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ExecStoreProcT(string connKey, string stpName, params SqlParameter[] paras)
        {
            DataTable rt = null;
            using (DataSet myDs = ExecStoreProcS(connKey, stpName, paras))
            {
                if (myDs != null)
                    rt = myDs.Tables[0];
            }
            return rt;
        }

        ///<summary>获取分页的Datareader对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="fieldList">查询的字段列表，不含“SELECT”</param>
        /// <param name="tableAndCondition">查询的表和条件，不含“FROM”</param>
        /// <param name="orderWay">排序方式，不含“ORDER BY”</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">需要显示第几页</param>
        /// <param name="rsCount">返回得到总记录条数，如果出错返回-1 </param>
        /// <param name="paras">参数列表</param>
        /// <returns>DbDataReader对象，如果出错返回null</returns>
        public static SqlDataReader GetPageReader(string connKey, string fieldList, string tableAndCondition,
            string orderWay, int pageSize, int pageIndex, out int rsCount, params SqlParameter[] paras)
        {
            SqlDataReader myDr = GetDataReader(connKey, getPageSql(fieldList, tableAndCondition, orderWay, pageSize, pageIndex, true), paras);
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
        public static SqlDataReader GetPageReader(string connKey, string fieldList, string tableAndCondition,
            string orderWay, int pageSize, int pageIndex, params SqlParameter[] paras)
        {
            return GetDataReader(connKey, getPageSql(fieldList, tableAndCondition, orderWay, pageSize, pageIndex, false), paras);
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
        public static DataTable GetPageTable(string connKey, string fieldList, string tableAndCondition,
            string orderWay, int pageSize, int pageIndex, out int rsCount, params SqlParameter[] paras)
        {
            DataSet rt = GetDataSet(connKey, getPageSql(fieldList, tableAndCondition, orderWay, pageSize, pageIndex, true), paras);
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

        /// <summary>获取分页数据的Table对象
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="fieldList">查询的字段列表，不含“SELECT”</param>
        /// <param name="tableAndCondition">查询的表和条件，不含“FROM”</param>
        /// <param name="orderWay">排序方式，不含“ORDER BY”</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">需要显示第几页</param>
        /// <param name="paras">参数列表</param>
        /// <returns>Table对象</returns>
        public static DataTable GetPageTable(string connKey, string fieldList, string tableAndCondition,
            string orderWay, int pageSize, int pageIndex, params SqlParameter[] paras)
        {
            return GetDataTable(connKey, getPageSql(fieldList, tableAndCondition, orderWay, pageSize, pageIndex, false), paras);
        }

        /// <summary> 批量插入数据表数据
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="sTable">存有数据的内存数据表</param>
        /// <param name="dTableName">数据库目标数据表</param>
        /// <param name="withTran">是否启用事务</param>
        /// <param name="maps" >传入添加字段的数组  索引单数为源数据的列名   索引为双数的时候为对应单数的目标列目标列名</param>
        /// <returns>bool值，表示是否成功</returns>
        public static bool BlockInsertTable(string connKey, DataTable sTable, string dTableName, bool withTran, params string[] maps)
        {
            //todo 验证两边列数目不相同是否能执行 给手机查一下对对号和昵称
            bool rt = false;
            using (SqlBulkCopy sqlcopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings[connKey].ConnectionString, withTran ? SqlBulkCopyOptions.UseInternalTransaction : SqlBulkCopyOptions.Default))
            {
                sqlcopy.DestinationTableName = dTableName;
                sqlcopy.BatchSize = 1000;
                if (maps != null)
                {
                    for (int i = 0; i < maps.Length; i += 2)
                        sqlcopy.ColumnMappings.Add(maps[i], maps[i + 1]);
                }
                try
                {
                    sqlcopy.WriteToServer(sTable);
                    rt = true;
                }
                catch (SqlException err)
                {
                    saveProcError(connKey, 2, "BulkCopy", err.Message, null);
                }
                finally
                {
                    sqlcopy.Close();
                }
            }
            return rt;
        }

        //todo这个方法似乎不好使
        /// <summary>保存数据集中的修改到数据库
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="sqlStr">填充数据集使用的select sql语句（语句中的字段要跟数据集中每个表的字段完全对应，且无需条件）</param>
        /// <param name="Ds">需要更新的数据集</param>
        /// <returns></returns>
        public static bool SaveDataSet(string connKey, string sqlStr, DataSet Ds)
        {
            using (SqlConnection myCn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString))
            {
                SqlCommand myCm = new SqlCommand(sqlStr, myCn);
                SqlDataAdapter myDa = new SqlDataAdapter(myCm);
                // SqlCommandBuilder myCmb = new SqlCommandBuilder(myDa);
                try
                {
                    myDa.Update(Ds);
                }
                catch (SqlException err)
                {
                    saveProcError(connKey, 3, sqlStr, err.Message);
                    return false;
                }
                return true;

            }
        }

        /// <summary>保存TataTable中的数据修改到数据库
        /// </summary>
        /// <param name="connKey">在配置文件中配置的数据库连接字符串的键值</param>
        /// <param name="sqlStr">获取DataTable中数据的sql语句（sql语句中的字段必须跟DataTable中字段完全相同，且无需条件）</param>
        /// <param name="Dt">要保存数据的TataTable</param>
        /// <returns></returns>
        public static bool SaveDataTable(string connKey, string sqlStr, DataTable Dt)
        {
            using (SqlConnection myCn = new SqlConnection(ConfigurationManager.ConnectionStrings[connKey].ConnectionString))
            {
                SqlCommand myCm = new SqlCommand(sqlStr, myCn);
                SqlDataAdapter myDa = new SqlDataAdapter(myCm);
                //SqlCommandBuilder myCmb = new SqlCommandBuilder(myDa);
                try
                {
                    myDa.Update(Dt);
                }
                catch (SqlException err)
                {
                    saveProcError(connKey, 3, sqlStr, err.Message);
                    return false;
                }
                return true;
            }
        }
        #endregion


        #region 将DataTable或DataReader的数据转换为实体类集合

        /// <summary>
        /// 将数据表转换为实体类。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
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
        public static List<T> ConvertDataReaderToEntityList<T>(SqlDataReader dr) where T : new()
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

            #region 代码性能差
            //var type = typeof(T);
            //var list = new List<T>();
            //if (!dr.HasRows)
            //{
            //    return list;
            //}

            //var pros = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //while (dr.Read())
            //{
            //    var t = new T();
            //    foreach (var p in pros)
            //    {
            //        try
            //        {
            //            //dr.GetOrdinal(p.Name)如果字段不存在会抛异常
            //            if (p.CanWrite)
            //            {
            //                if (dr.GetOrdinal(p.Name) != -1 && !Convert.IsDBNull(dr[p.Name]))
            //                {
            //                    p.SetValue(t, dr[p.Name], null);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            continue;
            //        }
            //    }
            //    list.Add(t);
            //}
            //return list; 
            #endregion
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
        public static T ConvertDataReaderToEntitySingle<T>(SqlDataReader dr) where T : new()
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

        /// <summary>
        /// 优化通过反射为对象进行赋值性能 作者：唐晓军
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<SqlDataReader, T> GetReader<T>(List<string> param)
        {
            Delegate resDelegate;
            if (!ExpressionCache.TryGetValue(typeof(T), out resDelegate))
            {
                // Get the indexer property of SqlDataReader 
                var indexerProperty = typeof(SqlDataReader).GetProperty("Item", new[] { typeof(string) });
                // List of statements in our dynamic method 
                var statements = new List<Expression>();
                // Instance type of target entity class 
                ParameterExpression instanceParam = Expression.Variable(typeof(T));
                // Parameter for the SqlDataReader object
                ParameterExpression readerParam = Expression.Parameter(typeof(SqlDataReader));

                // Create and assign new T to variable. Ex. var instance = new T(); 
                BinaryExpression createInstance = Expression.Assign(instanceParam, Expression.New(typeof(T)));
                statements.Add(createInstance);

                foreach (var property in typeof(T).GetProperties())
                {
                    if (!param.Contains(property.Name))
                        continue;
                    // instance.Property 
                    MemberExpression getProperty = Expression.Property(instanceParam, property);
                    // row[property] The assumption is, column names are the 
                    // same as PropertyInfo names of T 
                    IndexExpression readValue = Expression.MakeIndex(readerParam, indexerProperty, new[] { Expression.Constant(property.Name) });

                    // 为属性赋值
                    BinaryExpression assignProperty = Expression.Assign(getProperty, Expression.Convert(readValue, property.PropertyType));

                    statements.Add(assignProperty);
                }
                var returnStatement = instanceParam;
                statements.Add(returnStatement);

                var body = Expression.Block(instanceParam.Type,
                    new[] { instanceParam }, statements.ToArray());

                var lambda = Expression.Lambda<Func<SqlDataReader, T>>(body, readerParam);
                resDelegate = lambda.Compile();

                // 将动态方法保存到缓存中
                ExpressionCache[typeof(T)] = resDelegate;
            }
            return (Func<SqlDataReader, T>)resDelegate;
        }

        /// <summary>
        /// SqlDataReader转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ReadList<T>(SqlDataReader reader)
        {
            var list = new List<T>();
            List<string> proNames = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                proNames.Add(reader.GetName(i));
            }
            Func<SqlDataReader, T> readRow = GetReader<T>(proNames);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(readRow(reader));//注意数据类型必须与数据库一致
                }
            }
            return list;
        }
    }
}
