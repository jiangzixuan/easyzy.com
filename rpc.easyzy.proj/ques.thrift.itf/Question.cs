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
  public partial class Question : TBase
  {
    private string _Id;
    private short _CourseId;
    private short _TypeId;
    private string _TypeName;
    private string _SubjectId;
    private short _GradeId;
    private short _DiffType;
    private double _Diff;
    private string _PaperId;
    private short _PaperYear;
    private short _PaperTypeId;
    private sbyte _HasChildren;
    private string _QuesBody;
    private string _QuesAnswer;
    private string _QuesParse;
    private string _KPoints;
    private string _CPoints;
    private List<CQuestion> _CQuestions;
    private QuesOption _QuesOption;

    public string Id
    {
      get
      {
        return _Id;
      }
      set
      {
        __isset.Id = true;
        this._Id = value;
      }
    }

    public short CourseId
    {
      get
      {
        return _CourseId;
      }
      set
      {
        __isset.CourseId = true;
        this._CourseId = value;
      }
    }

    public short TypeId
    {
      get
      {
        return _TypeId;
      }
      set
      {
        __isset.TypeId = true;
        this._TypeId = value;
      }
    }

    public string TypeName
    {
      get
      {
        return _TypeName;
      }
      set
      {
        __isset.TypeName = true;
        this._TypeName = value;
      }
    }

    public string SubjectId
    {
      get
      {
        return _SubjectId;
      }
      set
      {
        __isset.SubjectId = true;
        this._SubjectId = value;
      }
    }

    public short GradeId
    {
      get
      {
        return _GradeId;
      }
      set
      {
        __isset.GradeId = true;
        this._GradeId = value;
      }
    }

    public short DiffType
    {
      get
      {
        return _DiffType;
      }
      set
      {
        __isset.DiffType = true;
        this._DiffType = value;
      }
    }

    public double Diff
    {
      get
      {
        return _Diff;
      }
      set
      {
        __isset.Diff = true;
        this._Diff = value;
      }
    }

    public string PaperId
    {
      get
      {
        return _PaperId;
      }
      set
      {
        __isset.PaperId = true;
        this._PaperId = value;
      }
    }

    public short PaperYear
    {
      get
      {
        return _PaperYear;
      }
      set
      {
        __isset.PaperYear = true;
        this._PaperYear = value;
      }
    }

    public short PaperTypeId
    {
      get
      {
        return _PaperTypeId;
      }
      set
      {
        __isset.PaperTypeId = true;
        this._PaperTypeId = value;
      }
    }

    public sbyte HasChildren
    {
      get
      {
        return _HasChildren;
      }
      set
      {
        __isset.HasChildren = true;
        this._HasChildren = value;
      }
    }

    public string QuesBody
    {
      get
      {
        return _QuesBody;
      }
      set
      {
        __isset.QuesBody = true;
        this._QuesBody = value;
      }
    }

    public string QuesAnswer
    {
      get
      {
        return _QuesAnswer;
      }
      set
      {
        __isset.QuesAnswer = true;
        this._QuesAnswer = value;
      }
    }

    public string QuesParse
    {
      get
      {
        return _QuesParse;
      }
      set
      {
        __isset.QuesParse = true;
        this._QuesParse = value;
      }
    }

    public string KPoints
    {
      get
      {
        return _KPoints;
      }
      set
      {
        __isset.KPoints = true;
        this._KPoints = value;
      }
    }

    public string CPoints
    {
      get
      {
        return _CPoints;
      }
      set
      {
        __isset.CPoints = true;
        this._CPoints = value;
      }
    }

    public List<CQuestion> CQuestions
    {
      get
      {
        return _CQuestions;
      }
      set
      {
        __isset.CQuestions = true;
        this._CQuestions = value;
      }
    }

    public QuesOption QuesOption
    {
      get
      {
        return _QuesOption;
      }
      set
      {
        __isset.QuesOption = true;
        this._QuesOption = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool Id;
      public bool CourseId;
      public bool TypeId;
      public bool TypeName;
      public bool SubjectId;
      public bool GradeId;
      public bool DiffType;
      public bool Diff;
      public bool PaperId;
      public bool PaperYear;
      public bool PaperTypeId;
      public bool HasChildren;
      public bool QuesBody;
      public bool QuesAnswer;
      public bool QuesParse;
      public bool KPoints;
      public bool CPoints;
      public bool CQuestions;
      public bool QuesOption;
    }

    public Question() {
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
                Id = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.I16) {
                CourseId = iprot.ReadI16();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I16) {
                TypeId = iprot.ReadI16();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                TypeName = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.String) {
                SubjectId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.I16) {
                GradeId = iprot.ReadI16();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.I16) {
                DiffType = iprot.ReadI16();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 8:
              if (field.Type == TType.Double) {
                Diff = iprot.ReadDouble();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 9:
              if (field.Type == TType.String) {
                PaperId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 10:
              if (field.Type == TType.I16) {
                PaperYear = iprot.ReadI16();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 11:
              if (field.Type == TType.I16) {
                PaperTypeId = iprot.ReadI16();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 12:
              if (field.Type == TType.Byte) {
                HasChildren = iprot.ReadByte();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 13:
              if (field.Type == TType.String) {
                QuesBody = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 14:
              if (field.Type == TType.String) {
                QuesAnswer = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 15:
              if (field.Type == TType.String) {
                QuesParse = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 16:
              if (field.Type == TType.String) {
                KPoints = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 17:
              if (field.Type == TType.String) {
                CPoints = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 18:
              if (field.Type == TType.List) {
                {
                  CQuestions = new List<CQuestion>();
                  TList _list0 = iprot.ReadListBegin();
                  for( int _i1 = 0; _i1 < _list0.Count; ++_i1)
                  {
                    CQuestion _elem2;
                    _elem2 = new CQuestion();
                    _elem2.Read(iprot);
                    CQuestions.Add(_elem2);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 19:
              if (field.Type == TType.Struct) {
                QuesOption = new QuesOption();
                QuesOption.Read(iprot);
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
        TStruct struc = new TStruct("Question");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Id != null && __isset.Id) {
          field.Name = "Id";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Id);
          oprot.WriteFieldEnd();
        }
        if (__isset.CourseId) {
          field.Name = "CourseId";
          field.Type = TType.I16;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteI16(CourseId);
          oprot.WriteFieldEnd();
        }
        if (__isset.TypeId) {
          field.Name = "TypeId";
          field.Type = TType.I16;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteI16(TypeId);
          oprot.WriteFieldEnd();
        }
        if (TypeName != null && __isset.TypeName) {
          field.Name = "TypeName";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(TypeName);
          oprot.WriteFieldEnd();
        }
        if (SubjectId != null && __isset.SubjectId) {
          field.Name = "SubjectId";
          field.Type = TType.String;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(SubjectId);
          oprot.WriteFieldEnd();
        }
        if (__isset.GradeId) {
          field.Name = "GradeId";
          field.Type = TType.I16;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteI16(GradeId);
          oprot.WriteFieldEnd();
        }
        if (__isset.DiffType) {
          field.Name = "DiffType";
          field.Type = TType.I16;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          oprot.WriteI16(DiffType);
          oprot.WriteFieldEnd();
        }
        if (__isset.Diff) {
          field.Name = "Diff";
          field.Type = TType.Double;
          field.ID = 8;
          oprot.WriteFieldBegin(field);
          oprot.WriteDouble(Diff);
          oprot.WriteFieldEnd();
        }
        if (PaperId != null && __isset.PaperId) {
          field.Name = "PaperId";
          field.Type = TType.String;
          field.ID = 9;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(PaperId);
          oprot.WriteFieldEnd();
        }
        if (__isset.PaperYear) {
          field.Name = "PaperYear";
          field.Type = TType.I16;
          field.ID = 10;
          oprot.WriteFieldBegin(field);
          oprot.WriteI16(PaperYear);
          oprot.WriteFieldEnd();
        }
        if (__isset.PaperTypeId) {
          field.Name = "PaperTypeId";
          field.Type = TType.I16;
          field.ID = 11;
          oprot.WriteFieldBegin(field);
          oprot.WriteI16(PaperTypeId);
          oprot.WriteFieldEnd();
        }
        if (__isset.HasChildren) {
          field.Name = "HasChildren";
          field.Type = TType.Byte;
          field.ID = 12;
          oprot.WriteFieldBegin(field);
          oprot.WriteByte(HasChildren);
          oprot.WriteFieldEnd();
        }
        if (QuesBody != null && __isset.QuesBody) {
          field.Name = "QuesBody";
          field.Type = TType.String;
          field.ID = 13;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(QuesBody);
          oprot.WriteFieldEnd();
        }
        if (QuesAnswer != null && __isset.QuesAnswer) {
          field.Name = "QuesAnswer";
          field.Type = TType.String;
          field.ID = 14;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(QuesAnswer);
          oprot.WriteFieldEnd();
        }
        if (QuesParse != null && __isset.QuesParse) {
          field.Name = "QuesParse";
          field.Type = TType.String;
          field.ID = 15;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(QuesParse);
          oprot.WriteFieldEnd();
        }
        if (KPoints != null && __isset.KPoints) {
          field.Name = "KPoints";
          field.Type = TType.String;
          field.ID = 16;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(KPoints);
          oprot.WriteFieldEnd();
        }
        if (CPoints != null && __isset.CPoints) {
          field.Name = "CPoints";
          field.Type = TType.String;
          field.ID = 17;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(CPoints);
          oprot.WriteFieldEnd();
        }
        if (CQuestions != null && __isset.CQuestions) {
          field.Name = "CQuestions";
          field.Type = TType.List;
          field.ID = 18;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, CQuestions.Count));
            foreach (CQuestion _iter3 in CQuestions)
            {
              _iter3.Write(oprot);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (QuesOption != null && __isset.QuesOption) {
          field.Name = "QuesOption";
          field.Type = TType.Struct;
          field.ID = 19;
          oprot.WriteFieldBegin(field);
          QuesOption.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("Question(");
      bool __first = true;
      if (Id != null && __isset.Id) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Id: ");
        __sb.Append(Id);
      }
      if (__isset.CourseId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CourseId: ");
        __sb.Append(CourseId);
      }
      if (__isset.TypeId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("TypeId: ");
        __sb.Append(TypeId);
      }
      if (TypeName != null && __isset.TypeName) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("TypeName: ");
        __sb.Append(TypeName);
      }
      if (SubjectId != null && __isset.SubjectId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("SubjectId: ");
        __sb.Append(SubjectId);
      }
      if (__isset.GradeId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("GradeId: ");
        __sb.Append(GradeId);
      }
      if (__isset.DiffType) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DiffType: ");
        __sb.Append(DiffType);
      }
      if (__isset.Diff) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Diff: ");
        __sb.Append(Diff);
      }
      if (PaperId != null && __isset.PaperId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("PaperId: ");
        __sb.Append(PaperId);
      }
      if (__isset.PaperYear) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("PaperYear: ");
        __sb.Append(PaperYear);
      }
      if (__isset.PaperTypeId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("PaperTypeId: ");
        __sb.Append(PaperTypeId);
      }
      if (__isset.HasChildren) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("HasChildren: ");
        __sb.Append(HasChildren);
      }
      if (QuesBody != null && __isset.QuesBody) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("QuesBody: ");
        __sb.Append(QuesBody);
      }
      if (QuesAnswer != null && __isset.QuesAnswer) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("QuesAnswer: ");
        __sb.Append(QuesAnswer);
      }
      if (QuesParse != null && __isset.QuesParse) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("QuesParse: ");
        __sb.Append(QuesParse);
      }
      if (KPoints != null && __isset.KPoints) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("KPoints: ");
        __sb.Append(KPoints);
      }
      if (CPoints != null && __isset.CPoints) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CPoints: ");
        __sb.Append(CPoints);
      }
      if (CQuestions != null && __isset.CQuestions) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CQuestions: ");
        __sb.Append(CQuestions);
      }
      if (QuesOption != null && __isset.QuesOption) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("QuesOption: ");
        __sb.Append(QuesOption== null ? "<null>" : QuesOption.ToString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}