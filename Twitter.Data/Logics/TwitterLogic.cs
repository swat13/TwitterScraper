using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Twitter.Data.Models;

namespace Twitter.Data.Logics
{
    public class TwitterLogic
    {
        private readonly IMongoDatabase _db;
        private readonly string _tableName;
        public TwitterLogic()
        {
            var client = new MongoClient();
            this._tableName = StaticsClass._tableName;
            _db = client.GetDatabase(StaticsClass._dbName);

        }

        public void InsertCollection<T>(T record)
        {
            var collection = _db.GetCollection<T>(_tableName);
            collection.InsertOne(record);

        }

        public List<T> LoadRecords<T>()
        {
            var collection = _db.GetCollection<T>(_tableName);
            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordByTopicType<T>(int topicType)
        {
            var collection = _db.GetCollection<T>(_tableName);
            var filter = Builders<T>.Filter.Eq("topicType", topicType);
            return collection.Find(filter).FirstOrDefault();
        }

        public void UpsertRecordByTopic<T>(int topicType, T record)
        {
            var collection = _db.GetCollection<T>(_tableName);
            var filter = Builders<T>.Filter.Eq("topicType", topicType);
            collection.ReplaceOne(filter, record, new UpdateOptions { IsUpsert = true });
        }


        public void InsertRecordIfNotExsistByTopic<T>(int topicType, T record)
        {
            var collection = _db.GetCollection<T>(_tableName);
            var currentRecord = LoadRecordByTopicType<T>(topicType);
            if (currentRecord == null)
            {
                collection.InsertOne(record);
            }
        }

        public void InitiateTopics()
        {
            var searchHistoryTrump = new SearchModel
            {
                topicType = (int)Enums.Topic.Trump,
                dailyCountList = new List<DailyCountModel>
                    {new DailyCountModel {date = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd"), count = 0}},
                nextResultUrl = "?q=Trump&count=100&include_entities=1&result_type=recent"
            };

            var searchHistoryLady = new SearchModel
            {
                topicType = (int)Enums.Topic.Lady,
                dailyCountList = new List<DailyCountModel>
                    {new DailyCountModel {date = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd"), count = 0}},
                nextResultUrl = "?q=Lay%20Gaga&count=100&include_entities=1&result_type=recent"
            };
            var searchHistoryIsis = new SearchModel
            {
                topicType = (int)Enums.Topic.Isis,
                dailyCountList = new List<DailyCountModel>
                    {new DailyCountModel {date = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd"), count = 0}},
                nextResultUrl = "?q=ISIS&count=100&include_entities=1&result_type=recent"
            };
            var searchHistoryESports = new SearchModel
            {
                topicType = (int)Enums.Topic.ESports,
                dailyCountList = new List<DailyCountModel>
                {
                    new DailyCountModel {date = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd"), count = 0}
                },
                nextResultUrl = "?q=Esports&count=100&include_entities=1&result_type=recent"
            };

            InsertRecordIfNotExsistByTopic((int)Enums.Topic.Trump, searchHistoryTrump);
            InsertRecordIfNotExsistByTopic((int)Enums.Topic.Isis, searchHistoryIsis);
            InsertRecordIfNotExsistByTopic((int)Enums.Topic.Lady, searchHistoryLady);
            InsertRecordIfNotExsistByTopic((int)Enums.Topic.ESports, searchHistoryESports);

        }
    }

}


