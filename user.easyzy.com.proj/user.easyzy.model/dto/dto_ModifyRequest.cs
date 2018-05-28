using user.easyzy.model.entity;

namespace user.easyzy.model.dto
{
    public class dto_ModifyRequest : T_ModifyRequest
    {
        public string FromSchoolName { get; set; }

        public string ToSchoolName { get; set; }

        public string CreateDateStr { get; set; }

    }
}
