using System;
using System.Runtime.Serialization;

namespace Twitter.Data.Models
{
    [DataContract]
    public class MyFaults
    {
        [DataContract(Name = "ErrorCode")]
        public enum ErrorCode
        {
            [EnumMember] Unknown = 0,
            [EnumMember] DBError = 2,

        }

        [DataMember] public ErrorCode errorCode = ErrorCode.Unknown;

        [DataMember] public double maintenanceTime;

        [DataMember] public string message = "";

        public MyFaults(ErrorCode errorCode, string message, Exception ex)
        {
            this.errorCode = errorCode;
            this.message = message;
        }
    }
}