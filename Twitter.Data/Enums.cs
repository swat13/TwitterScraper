using System.Runtime.Serialization;

namespace Twitter.Data.Logics
{
    [DataContract]
    public class Enums
    {
        [DataContract(Name = "TopicTypeEnums")]
        public enum Topic
        {
            [EnumMember] Trump = 0,
            [EnumMember] Lady = 1,
            [EnumMember] Isis = 2,
            [EnumMember] ESports = 3,
        }



    }
}