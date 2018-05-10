namespace csharp ques.thrift.itf

struct QuesOption
{
	1: string QuesId,
	2: string OptionA,
	3: string OptionB,
	4: string OptionC,
	5: string OptionD,
	6: string OptionE,
	7: string OptionF,
	8: string OptionG
}

struct CQuestion
{
	1: string Id,
	2: string ParentId,
	3: i16 OrderIndex,
	4: i16 TypeId,
	5: string TypeName,
	6: i16 DiffType,
	7: double Diff,
	8: string QuesBody,
	9: string QuesAnswer,
	10: string QuesParse,
	11: string KPoints,
	12: string CPoints,
	13: QuesOption QuesOption
}

struct Question
{
	1: string Id,
    2: i16 CourseId,
	3: i16 TypeId,
	4: string TypeName,
	5: string SubjectId,
	6: i16 GradeId,
	7: i16 DiffType,
	8: double Diff,
	9: string PaperId,
    10: i16 PaperYear,
	11: i16 PaperTypeId,
	12: i8 HasChildren,
	13: string QuesBody,
	14: string QuesAnswer,
	15: string QuesParse,
	16: string KPoints,
	17: string CPoints,
	18: list<CQuestion> CQuestions,
	19: QuesOption QuesOption
}

struct QuesType
{
	1: i16 Id,
	2: string Name,
	3: i8 IsSelect,
	4: i8 IsMultiple
}

service QuesService{

    Question GetQuestion(1:string quesId)

    list<Question> QueryQuestions(1:i16 courseId, 2:i16 typeId, 3:i16 diffType, 4:i16 paperTypeId, 5:i16 kpId, 6:i16 cpId, 7:i16 pageIndex, 8:i16 pageSize)

	i8 OfflineQues(1:string quesId)
}
