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

namespace userthrift.itf.model
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class address : TBase
  {
    private string _Line1;
    private string _Line2;

    public string Line1
    {
      get
      {
        return _Line1;
      }
      set
      {
        __isset.Line1 = true;
        this._Line1 = value;
      }
    }

    public string Line2
    {
      get
      {
        return _Line2;
      }
      set
      {
        __isset.Line2 = true;
        this._Line2 = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool Line1;
      public bool Line2;
    }

    public address() {
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
                Line1 = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Line2 = iprot.ReadString();
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
        TStruct struc = new TStruct("address");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Line1 != null && __isset.Line1) {
          field.Name = "Line1";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Line1);
          oprot.WriteFieldEnd();
        }
        if (Line2 != null && __isset.Line2) {
          field.Name = "Line2";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Line2);
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
      StringBuilder __sb = new StringBuilder("address(");
      bool __first = true;
      if (Line1 != null && __isset.Line1) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Line1: ");
        __sb.Append(Line1);
      }
      if (Line2 != null && __isset.Line2) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Line2: ");
        __sb.Append(Line2);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
