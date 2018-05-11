<%@ webhandler Language="C#" class="Upload" %>
using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using LitJson;
using jqzy.common;
using jqzy.model;
using jqzy.bll;

/// <summary>
/// 修改upload_json.ashx，将图片通过upload.jqzy.zxxk.com上传到服务器
/// 2016/12/27
/// </summary>
public class Upload : IHttpHandler
{
    private HttpContext context;

    public void ProcessRequest(HttpContext context)
    {
        HttpPostedFile imgFile = context.Request.Files["imgFile"];
        if (imgFile == null)
        {
            showError("No file selected.");
        }
        string result = "";
        
        result = FileUploader.UploadToServer(2, imgFile);
        
        var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseEntity<List<string>>>(result);
        Hashtable hash = new Hashtable();
        hash["error"] = 0;
        hash["url"] = r.Data[0];
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        context.Response.End();
    }



    private void showError(string message)
    {
        Hashtable hash = new Hashtable();
        hash["error"] = 1;
        hash["message"] = message;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}

public class ResponseEntity<T>
{

    public int Code { get; set; }
    
    public int BussCode { get; set; }
    
    public string Message { get; set; }
    
    public T Data { get; set; }

    public ResponseEntity()
    {
        BussCode = (int)ResponseBussCode.Default;
    }
}

public enum ResponseBussCode
{
    Default = 0,

    Success = 1000,

    Error = 1001,
 
    Evaluating = 1002,

    NoSumbit = 1003
}
public class RemarkAttribute : Attribute
{
    private string m_remark;
    public RemarkAttribute(string remark)
    {
        this.m_remark = remark;
    }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark
    {
        get { return m_remark; }
    }

}