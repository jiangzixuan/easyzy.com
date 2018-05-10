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

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class QuesOption : TBase
  {
    private string _QuesId;
    private string _OptionA;
    private string _OptionB;
    private string _OptionC;
    private string _OptionD;
    private string _OptionE;
    private string _OptionF;
    private string _OptionG;

    public string QuesId
    {
      get
      {
        return _QuesId;
      }
      set
      {
        __isset.QuesId = true;
        this._QuesId = value;
      }
    }

    public string OptionA
    {
      get
      {
        return _OptionA;
      }
      set
      {
        __isset.OptionA = true;
        this._OptionA = value;
      }
    }

    public string OptionB
    {
      get
      {
        return _OptionB;
      }
      set
      {
        __isset.OptionB = true;
        this._OptionB = value;
      }
    }

    public string OptionC
    {
      get
      {
        return _OptionC;
      }
      set
      {
        __isset.OptionC = true;
        this._OptionC = value;
      }
    }

    public string OptionD
    {
      get
      {
        return _OptionD;
      }
      set
      {
        __isset.OptionD = true;
        this._OptionD = value;
      }
    }

    public string OptionE
    {
      get
      {
        return _OptionE;
      }
      set
      {
        __isset.OptionE = true;
        this._OptionE = value;
      }
    }

    public string OptionF
    {
      get
      {
        return _OptionF;
      }
      set
      {
        __isset.OptionF = true;
        this._OptionF = value;
      }
    }

    public string OptionG
    {
      get
      {
        return _OptionG;
      }
      set
      {
        __isset.OptionG = true;
        this._OptionG = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool QuesId;
      public bool OptionA;
      public bool OptionB;
      public bool OptionC;
      public bool OptionD;
      public bool OptionE;
      public bool OptionF;
      public bool OptionG;
    }

    public QuesOption() {
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
            case 2:
              if (field.Type == TType.String) {
                OptionA = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                OptionB = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                OptionC = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.String) {
                OptionD = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.String) {
                OptionE = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.String) {
                OptionF = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 8:
              if (field.Type == TType.String) {
                OptionG = iprot.ReadString();
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
        TStruct struc = new TStruct("QuesOption");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (QuesId != null && __isset.QuesId) {
          field.Name = "QuesId";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(QuesId);
          oprot.WriteFieldEnd();
        }
        if (OptionA != null && __isset.OptionA) {
          field.Name = "OptionA";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(OptionA);
          oprot.WriteFieldEnd();
        }
        if (OptionB != null && __isset.OptionB) {
          field.Name = "OptionB";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(OptionB);
          oprot.WriteFieldEnd();
        }
        if (OptionC != null && __isset.OptionC) {
          field.Name = "OptionC";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(OptionC);
          oprot.WriteFieldEnd();
        }
        if (OptionD != null && __isset.OptionD) {
          field.Name = "OptionD";
          field.Type = TType.String;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(OptionD);
          oprot.WriteFieldEnd();
        }
        if (OptionE != null && __isset.OptionE) {
          field.Name = "OptionE";
          field.Type = TType.String;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(OptionE);
          oprot.WriteFieldEnd();
        }
        if (OptionF != null && __isset.OptionF) {
          field.Name = "OptionF";
          field.Type = TType.String;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(OptionF);
          oprot.WriteFieldEnd();
        }
        if (OptionG != null && __isset.OptionG) {
          field.Name = "OptionG";
          field.Type = TType.String;
          field.ID = 8;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(OptionG);
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
      StringBuilder __sb = new StringBuilder("QuesOption(");
      bool __first = true;
      if (QuesId != null && __isset.QuesId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("QuesId: ");
        __sb.Append(QuesId);
      }
      if (OptionA != null && __isset.OptionA) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OptionA: ");
        __sb.Append(OptionA);
      }
      if (OptionB != null && __isset.OptionB) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OptionB: ");
        __sb.Append(OptionB);
      }
      if (OptionC != null && __isset.OptionC) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OptionC: ");
        __sb.Append(OptionC);
      }
      if (OptionD != null && __isset.OptionD) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OptionD: ");
        __sb.Append(OptionD);
      }
      if (OptionE != null && __isset.OptionE) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OptionE: ");
        __sb.Append(OptionE);
      }
      if (OptionF != null && __isset.OptionF) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OptionF: ");
        __sb.Append(OptionF);
      }
      if (OptionG != null && __isset.OptionG) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OptionG: ");
        __sb.Append(OptionG);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}