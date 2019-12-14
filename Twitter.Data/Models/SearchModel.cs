using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Twitter.Data.Models
{
    public class SearchModel
    {
        [BsonId]
        public Guid id { get; set; }
        public int topicType { get; set; }
        public string nextResultUrl { get; set; }
        public List<DailyCountModel> dailyCountList { get; set; }
    }
}