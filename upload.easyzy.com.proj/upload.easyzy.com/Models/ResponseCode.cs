using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace upload.easyzy.com.Models
{
    /// <summary>执行结果状态码
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        Success = 1200,
        /// <summary>
        /// 请求接收，将被异步处理
        /// </summary>
        SuccessAsys = 1202,
        /// <summary>
        /// 用户未认证，请求失败 
        /// </summary>
        Unauthorized = 1401,
        /// <summary>
        /// 用户无权限访问该资源，请求失败
        /// </summary>
        Forbidden = 1403,
        /// <summary>
        /// 签名错误
        /// </summary>
        SignError = 1404,
        /// <summary>
        /// 时间戳错误
        /// </summary>
        TimeStempInvalid = 1405,

        /// <summary>
        /// 请求的数据不存在
        /// </summary>
        DataNotExist = 1406,
        /// <summary>
        /// 请求的参数存在错误
        /// </summary>
        ParamsError = 1407,
        /// <summary>
        /// 服务器错误，确认状态并报告问题
        /// </summary>
        ServerError = 1500,

        /// <summary>
        /// 业务错误
        /// </summary>
        Error = 1501,

        /// <summary>
        /// 非法用户
        /// </summary>
        IllegalUser= 1502

        //422 Unprocessable Entity: 请求被服务器正确解析，但是包含无效字段
        //429 Too Many Requests: 因为访问频繁，你已经被限制访问，稍后重试
        //500 Internal Server Error: 服务器错误，确认状态并报告问题
    }

    /// <summary>
    /// 业务状态码
    /// </summary>
    public enum ResponseBussCode
    {
        /// <summary>
        /// 业务默认状态、没有意义
        /// </summary>
        Default = 0,
        /// <summary>
        /// 业务成功状态
        /// </summary>
        Success = 1000,
        /// <summary>
        /// 业务失败状态
        /// </summary>
        Error = 1001
    }
}