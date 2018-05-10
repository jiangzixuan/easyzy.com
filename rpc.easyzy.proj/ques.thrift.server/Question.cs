using System;
using System.Collections.Generic;

namespace ques.thrift.server
{
    public class Question : itf.QuesService.Iface
    {
        public itf.Question GetQuestion(string quesId)
        {
            throw new NotImplementedException();
        }

        public sbyte OfflineQues(string quesId)
        {
            throw new NotImplementedException();
        }

        public List<itf.Question> QueryQuestions(short courseId, short typeId, short diffType, short paperTypeId, short kpId, short cpId, short pageIndex, short pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
