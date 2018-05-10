/**
 * Autogenerated by Thrift Compiler (0.11.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace ques.thrift.itf
{
  public partial class QuesService {
    public interface ISync {
      Question GetQuestion(string quesId);
      List<Question> QueryQuestions(short courseId, short typeId, short diffType, short paperTypeId, short kpId, short cpId, short pageIndex, short pageSize);
      sbyte OfflineQues(string quesId);
    }

    public interface Iface : ISync {
      #if SILVERLIGHT
      IAsyncResult Begin_GetQuestion(AsyncCallback callback, object state, string quesId);
      Question End_GetQuestion(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_QueryQuestions(AsyncCallback callback, object state, short courseId, short typeId, short diffType, short paperTypeId, short kpId, short cpId, short pageIndex, short pageSize);
      List<Question> End_QueryQuestions(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_OfflineQues(AsyncCallback callback, object state, string quesId);
      sbyte End_OfflineQues(IAsyncResult asyncResult);
      #endif
    }

    public class Client : IDisposable, Iface {
      public Client(TProtocol prot) : this(prot, prot)
      {
      }

      public Client(TProtocol iprot, TProtocol oprot)
      {
        iprot_ = iprot;
        oprot_ = oprot;
      }

      protected TProtocol iprot_;
      protected TProtocol oprot_;
      protected int seqid_;

      public TProtocol InputProtocol
      {
        get { return iprot_; }
      }
      public TProtocol OutputProtocol
      {
        get { return oprot_; }
      }


      #region " IDisposable Support "
      private bool _IsDisposed;

      // IDisposable
      public void Dispose()
      {
        Dispose(true);
      }
      

      protected virtual void Dispose(bool disposing)
      {
        if (!_IsDisposed)
        {
          if (disposing)
          {
            if (iprot_ != null)
            {
              ((IDisposable)iprot_).Dispose();
            }
            if (oprot_ != null)
            {
              ((IDisposable)oprot_).Dispose();
            }
          }
        }
        _IsDisposed = true;
      }
      #endregion


      
      #if SILVERLIGHT
      public IAsyncResult Begin_GetQuestion(AsyncCallback callback, object state, string quesId)
      {
        return send_GetQuestion(callback, state, quesId);
      }

      public Question End_GetQuestion(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        return recv_GetQuestion();
      }

      #endif

      public Question GetQuestion(string quesId)
      {
        #if !SILVERLIGHT
        send_GetQuestion(quesId);
        return recv_GetQuestion();

        #else
        var asyncResult = Begin_GetQuestion(null, null, quesId);
        return End_GetQuestion(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_GetQuestion(AsyncCallback callback, object state, string quesId)
      #else
      public void send_GetQuestion(string quesId)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("GetQuestion", TMessageType.Call, seqid_));
        GetQuestion_args args = new GetQuestion_args();
        args.QuesId = quesId;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public Question recv_GetQuestion()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        GetQuestion_result result = new GetQuestion_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        if (result.__isset.success) {
          return result.Success;
        }
        throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "GetQuestion failed: unknown result");
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_QueryQuestions(AsyncCallback callback, object state, short courseId, short typeId, short diffType, short paperTypeId, short kpId, short cpId, short pageIndex, short pageSize)
      {
        return send_QueryQuestions(callback, state, courseId, typeId, diffType, paperTypeId, kpId, cpId, pageIndex, pageSize);
      }

      public List<Question> End_QueryQuestions(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        return recv_QueryQuestions();
      }

      #endif

      public List<Question> QueryQuestions(short courseId, short typeId, short diffType, short paperTypeId, short kpId, short cpId, short pageIndex, short pageSize)
      {
        #if !SILVERLIGHT
        send_QueryQuestions(courseId, typeId, diffType, paperTypeId, kpId, cpId, pageIndex, pageSize);
        return recv_QueryQuestions();

        #else
        var asyncResult = Begin_QueryQuestions(null, null, courseId, typeId, diffType, paperTypeId, kpId, cpId, pageIndex, pageSize);
        return End_QueryQuestions(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_QueryQuestions(AsyncCallback callback, object state, short courseId, short typeId, short diffType, short paperTypeId, short kpId, short cpId, short pageIndex, short pageSize)
      #else
      public void send_QueryQuestions(short courseId, short typeId, short diffType, short paperTypeId, short kpId, short cpId, short pageIndex, short pageSize)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("QueryQuestions", TMessageType.Call, seqid_));
        QueryQuestions_args args = new QueryQuestions_args();
        args.CourseId = courseId;
        args.TypeId = typeId;
        args.DiffType = diffType;
        args.PaperTypeId = paperTypeId;
        args.KpId = kpId;
        args.CpId = cpId;
        args.PageIndex = pageIndex;
        args.PageSize = pageSize;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public List<Question> recv_QueryQuestions()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        QueryQuestions_result result = new QueryQuestions_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        if (result.__isset.success) {
          return result.Success;
        }
        throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "QueryQuestions failed: unknown result");
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_OfflineQues(AsyncCallback callback, object state, string quesId)
      {
        return send_OfflineQues(callback, state, quesId);
      }

      public sbyte End_OfflineQues(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        return recv_OfflineQues();
      }

      #endif

      public sbyte OfflineQues(string quesId)
      {
        #if !SILVERLIGHT
        send_OfflineQues(quesId);
        return recv_OfflineQues();

        #else
        var asyncResult = Begin_OfflineQues(null, null, quesId);
        return End_OfflineQues(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_OfflineQues(AsyncCallback callback, object state, string quesId)
      #else
      public void send_OfflineQues(string quesId)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("OfflineQues", TMessageType.Call, seqid_));
        OfflineQues_args args = new OfflineQues_args();
        args.QuesId = quesId;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public sbyte recv_OfflineQues()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        OfflineQues_result result = new OfflineQues_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        if (result.__isset.success) {
          return result.Success;
        }
        throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "OfflineQues failed: unknown result");
      }

    }
    public class Processor : TProcessor {
      public Processor(ISync iface)
      {
        iface_ = iface;
        processMap_["GetQuestion"] = GetQuestion_Process;
        processMap_["QueryQuestions"] = QueryQuestions_Process;
        processMap_["OfflineQues"] = OfflineQues_Process;
      }

      protected delegate void ProcessFunction(int seqid, TProtocol iprot, TProtocol oprot);
      private ISync iface_;
      protected Dictionary<string, ProcessFunction> processMap_ = new Dictionary<string, ProcessFunction>();

      public bool Process(TProtocol iprot, TProtocol oprot)
      {
        try
        {
          TMessage msg = iprot.ReadMessageBegin();
          ProcessFunction fn;
          processMap_.TryGetValue(msg.Name, out fn);
          if (fn == null) {
            TProtocolUtil.Skip(iprot, TType.Struct);
            iprot.ReadMessageEnd();
            TApplicationException x = new TApplicationException (TApplicationException.ExceptionType.UnknownMethod, "Invalid method name: '" + msg.Name + "'");
            oprot.WriteMessageBegin(new TMessage(msg.Name, TMessageType.Exception, msg.SeqID));
            x.Write(oprot);
            oprot.WriteMessageEnd();
            oprot.Transport.Flush();
            return true;
          }
          fn(msg.SeqID, iprot, oprot);
        }
        catch (IOException)
        {
          return false;
        }
        return true;
      }

      public void GetQuestion_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        GetQuestion_args args = new GetQuestion_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        GetQuestion_result result = new GetQuestion_result();
        try
        {
          result.Success = iface_.GetQuestion(args.QuesId);
          oprot.WriteMessageBegin(new TMessage("GetQuestion", TMessageType.Reply, seqid)); 
          result.Write(oprot);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
          TApplicationException x = new TApplicationException        (TApplicationException.ExceptionType.InternalError," Internal error.");
          oprot.WriteMessageBegin(new TMessage("GetQuestion", TMessageType.Exception, seqid));
          x.Write(oprot);
        }
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void QueryQuestions_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        QueryQuestions_args args = new QueryQuestions_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        QueryQuestions_result result = new QueryQuestions_result();
        try
        {
          result.Success = iface_.QueryQuestions(args.CourseId, args.TypeId, args.DiffType, args.PaperTypeId, args.KpId, args.CpId, args.PageIndex, args.PageSize);
          oprot.WriteMessageBegin(new TMessage("QueryQuestions", TMessageType.Reply, seqid)); 
          result.Write(oprot);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
          TApplicationException x = new TApplicationException        (TApplicationException.ExceptionType.InternalError," Internal error.");
          oprot.WriteMessageBegin(new TMessage("QueryQuestions", TMessageType.Exception, seqid));
          x.Write(oprot);
        }
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void OfflineQues_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        OfflineQues_args args = new OfflineQues_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        OfflineQues_result result = new OfflineQues_result();
        try
        {
          result.Success = iface_.OfflineQues(args.QuesId);
          oprot.WriteMessageBegin(new TMessage("OfflineQues", TMessageType.Reply, seqid)); 
          result.Write(oprot);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
          TApplicationException x = new TApplicationException        (TApplicationException.ExceptionType.InternalError," Internal error.");
          oprot.WriteMessageBegin(new TMessage("OfflineQues", TMessageType.Exception, seqid));
          x.Write(oprot);
        }
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class GetQuestion_args : TBase
    {
      private string _quesId;

      public string QuesId
      {
        get
        {
          return _quesId;
        }
        set
        {
          __isset.quesId = true;
          this._quesId = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool quesId;
      }

      public GetQuestion_args() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              case 1:
                if (field.Type == TType.String) {
                  QuesId = iprot.ReadString();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("GetQuestion_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (QuesId != null && __isset.quesId) {
            field.Name = "quesId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(QuesId);
            oprot.WriteFieldEnd();
          }
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("GetQuestion_args(");
        bool __first = true;
        if (QuesId != null && __isset.quesId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("QuesId: ");
          __sb.Append(QuesId);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class GetQuestion_result : TBase
    {
      private Question _success;

      public Question Success
      {
        get
        {
          return _success;
        }
        set
        {
          __isset.success = true;
          this._success = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool success;
      }

      public GetQuestion_result() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              case 0:
                if (field.Type == TType.Struct) {
                  Success = new Question();
                  Success.Read(iprot);
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("GetQuestion_result");
          oprot.WriteStructBegin(struc);
          TField field = new TField();

          if (this.__isset.success) {
            if (Success != null) {
              field.Name = "Success";
              field.Type = TType.Struct;
              field.ID = 0;
              oprot.WriteFieldBegin(field);
              Success.Write(oprot);
              oprot.WriteFieldEnd();
            }
          }
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("GetQuestion_result(");
        bool __first = true;
        if (Success != null && __isset.success) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Success: ");
          __sb.Append(Success== null ? "<null>" : Success.ToString());
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class QueryQuestions_args : TBase
    {
      private short _courseId;
      private short _typeId;
      private short _diffType;
      private short _paperTypeId;
      private short _kpId;
      private short _cpId;
      private short _pageIndex;
      private short _pageSize;

      public short CourseId
      {
        get
        {
          return _courseId;
        }
        set
        {
          __isset.courseId = true;
          this._courseId = value;
        }
      }

      public short TypeId
      {
        get
        {
          return _typeId;
        }
        set
        {
          __isset.typeId = true;
          this._typeId = value;
        }
      }

      public short DiffType
      {
        get
        {
          return _diffType;
        }
        set
        {
          __isset.diffType = true;
          this._diffType = value;
        }
      }

      public short PaperTypeId
      {
        get
        {
          return _paperTypeId;
        }
        set
        {
          __isset.paperTypeId = true;
          this._paperTypeId = value;
        }
      }

      public short KpId
      {
        get
        {
          return _kpId;
        }
        set
        {
          __isset.kpId = true;
          this._kpId = value;
        }
      }

      public short CpId
      {
        get
        {
          return _cpId;
        }
        set
        {
          __isset.cpId = true;
          this._cpId = value;
        }
      }

      public short PageIndex
      {
        get
        {
          return _pageIndex;
        }
        set
        {
          __isset.pageIndex = true;
          this._pageIndex = value;
        }
      }

      public short PageSize
      {
        get
        {
          return _pageSize;
        }
        set
        {
          __isset.pageSize = true;
          this._pageSize = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool courseId;
        public bool typeId;
        public bool diffType;
        public bool paperTypeId;
        public bool kpId;
        public bool cpId;
        public bool pageIndex;
        public bool pageSize;
      }

      public QueryQuestions_args() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              case 1:
                if (field.Type == TType.I16) {
                  CourseId = iprot.ReadI16();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 2:
                if (field.Type == TType.I16) {
                  TypeId = iprot.ReadI16();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 3:
                if (field.Type == TType.I16) {
                  DiffType = iprot.ReadI16();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 4:
                if (field.Type == TType.I16) {
                  PaperTypeId = iprot.ReadI16();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 5:
                if (field.Type == TType.I16) {
                  KpId = iprot.ReadI16();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 6:
                if (field.Type == TType.I16) {
                  CpId = iprot.ReadI16();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 7:
                if (field.Type == TType.I16) {
                  PageIndex = iprot.ReadI16();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 8:
                if (field.Type == TType.I16) {
                  PageSize = iprot.ReadI16();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("QueryQuestions_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (__isset.courseId) {
            field.Name = "courseId";
            field.Type = TType.I16;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteI16(CourseId);
            oprot.WriteFieldEnd();
          }
          if (__isset.typeId) {
            field.Name = "typeId";
            field.Type = TType.I16;
            field.ID = 2;
            oprot.WriteFieldBegin(field);
            oprot.WriteI16(TypeId);
            oprot.WriteFieldEnd();
          }
          if (__isset.diffType) {
            field.Name = "diffType";
            field.Type = TType.I16;
            field.ID = 3;
            oprot.WriteFieldBegin(field);
            oprot.WriteI16(DiffType);
            oprot.WriteFieldEnd();
          }
          if (__isset.paperTypeId) {
            field.Name = "paperTypeId";
            field.Type = TType.I16;
            field.ID = 4;
            oprot.WriteFieldBegin(field);
            oprot.WriteI16(PaperTypeId);
            oprot.WriteFieldEnd();
          }
          if (__isset.kpId) {
            field.Name = "kpId";
            field.Type = TType.I16;
            field.ID = 5;
            oprot.WriteFieldBegin(field);
            oprot.WriteI16(KpId);
            oprot.WriteFieldEnd();
          }
          if (__isset.cpId) {
            field.Name = "cpId";
            field.Type = TType.I16;
            field.ID = 6;
            oprot.WriteFieldBegin(field);
            oprot.WriteI16(CpId);
            oprot.WriteFieldEnd();
          }
          if (__isset.pageIndex) {
            field.Name = "pageIndex";
            field.Type = TType.I16;
            field.ID = 7;
            oprot.WriteFieldBegin(field);
            oprot.WriteI16(PageIndex);
            oprot.WriteFieldEnd();
          }
          if (__isset.pageSize) {
            field.Name = "pageSize";
            field.Type = TType.I16;
            field.ID = 8;
            oprot.WriteFieldBegin(field);
            oprot.WriteI16(PageSize);
            oprot.WriteFieldEnd();
          }
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("QueryQuestions_args(");
        bool __first = true;
        if (__isset.courseId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("CourseId: ");
          __sb.Append(CourseId);
        }
        if (__isset.typeId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("TypeId: ");
          __sb.Append(TypeId);
        }
        if (__isset.diffType) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("DiffType: ");
          __sb.Append(DiffType);
        }
        if (__isset.paperTypeId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("PaperTypeId: ");
          __sb.Append(PaperTypeId);
        }
        if (__isset.kpId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("KpId: ");
          __sb.Append(KpId);
        }
        if (__isset.cpId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("CpId: ");
          __sb.Append(CpId);
        }
        if (__isset.pageIndex) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("PageIndex: ");
          __sb.Append(PageIndex);
        }
        if (__isset.pageSize) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("PageSize: ");
          __sb.Append(PageSize);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class QueryQuestions_result : TBase
    {
      private List<Question> _success;

      public List<Question> Success
      {
        get
        {
          return _success;
        }
        set
        {
          __isset.success = true;
          this._success = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool success;
      }

      public QueryQuestions_result() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              case 0:
                if (field.Type == TType.List) {
                  {
                    Success = new List<Question>();
                    TList _list4 = iprot.ReadListBegin();
                    for( int _i5 = 0; _i5 < _list4.Count; ++_i5)
                    {
                      Question _elem6;
                      _elem6 = new Question();
                      _elem6.Read(iprot);
                      Success.Add(_elem6);
                    }
                    iprot.ReadListEnd();
                  }
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("QueryQuestions_result");
          oprot.WriteStructBegin(struc);
          TField field = new TField();

          if (this.__isset.success) {
            if (Success != null) {
              field.Name = "Success";
              field.Type = TType.List;
              field.ID = 0;
              oprot.WriteFieldBegin(field);
              {
                oprot.WriteListBegin(new TList(TType.Struct, Success.Count));
                foreach (Question _iter7 in Success)
                {
                  _iter7.Write(oprot);
                }
                oprot.WriteListEnd();
              }
              oprot.WriteFieldEnd();
            }
          }
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("QueryQuestions_result(");
        bool __first = true;
        if (Success != null && __isset.success) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Success: ");
          __sb.Append(Success);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class OfflineQues_args : TBase
    {
      private string _quesId;

      public string QuesId
      {
        get
        {
          return _quesId;
        }
        set
        {
          __isset.quesId = true;
          this._quesId = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool quesId;
      }

      public OfflineQues_args() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              case 1:
                if (field.Type == TType.String) {
                  QuesId = iprot.ReadString();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("OfflineQues_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (QuesId != null && __isset.quesId) {
            field.Name = "quesId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(QuesId);
            oprot.WriteFieldEnd();
          }
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("OfflineQues_args(");
        bool __first = true;
        if (QuesId != null && __isset.quesId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("QuesId: ");
          __sb.Append(QuesId);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class OfflineQues_result : TBase
    {
      private sbyte _success;

      public sbyte Success
      {
        get
        {
          return _success;
        }
        set
        {
          __isset.success = true;
          this._success = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool success;
      }

      public OfflineQues_result() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              case 0:
                if (field.Type == TType.Byte) {
                  Success = iprot.ReadByte();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("OfflineQues_result");
          oprot.WriteStructBegin(struc);
          TField field = new TField();

          if (this.__isset.success) {
            field.Name = "Success";
            field.Type = TType.Byte;
            field.ID = 0;
            oprot.WriteFieldBegin(field);
            oprot.WriteByte(Success);
            oprot.WriteFieldEnd();
          }
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("OfflineQues_result(");
        bool __first = true;
        if (__isset.success) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Success: ");
          __sb.Append(Success);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }

  }
}
