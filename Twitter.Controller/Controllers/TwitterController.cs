using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebPages;
using Twitter.Data;
using Twitter.Data.Logics;
using Twitter.Data.Models;

namespace Twitter.Controller.Controllers
{
    public class TwitterController
    {
        public List<SearchModel> GetHotTopicsCount()
        {
            try
            {
                var twitterDbLogic = new TwitterLogic();
                var searchResult = twitterDbLogic.LoadRecords<SearchModel>();
                return searchResult;
            }
            catch (Exception e)
            {
                throw new WebFaultException<MyFaults>(
                    new MyFaults(MyFaults.ErrorCode.Unknown, "100", e),
                    HttpStatusCode.BadRequest);
            }
        }

        public void StartSearchingService()
        {
            new TwitterLogic().InitiateTopics();

            var topicsTitles = new string[] { "Trump", "Lady Gaga", "ISIS", "Esports" };

            var task = new Task(() =>
            {
                for (var i = 0; i < Enum.GetNames(typeof(Enums.Topic)).Length; i++)
                    while (true)
                    {
                        var result = Search(topicsTitles[i], i);
                        Console.WriteLine(result);
                        if (result.Equals("DONE"))
                        {
                            break;
                        }

                        Thread.Sleep(6000);
                    }
            });
            task.Start();
        }

        private string Search(string searchTopic, int topicType)
        {
            var twitterDbLogic = new TwitterLogic();
            var currentRecord = twitterDbLogic.LoadRecordByTopicType<SearchModel>(topicType);

            var searchQuery = currentRecord.nextResultUrl.IsEmpty()
                ? "?result_type=recent&include_entities=0&count=100&q=" + searchTopic
                : currentRecord.nextResultUrl;
            var apiRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings.Get("Twitter_Search_Url") + searchQuery);
            const string timelineHeaderFormat = "{0} {1}";
            StaticsClass._token = StaticsClass._token.IsEmpty() ? new Authentication().RenewToken() : StaticsClass._token;
            apiRequest.Headers.Add("Authorization",
                string.Format(timelineHeaderFormat, "bearer", StaticsClass._token));
            apiRequest.Method = WebRequestMethods.Http.Get;
            var timeLineResponse = apiRequest.GetResponse();

            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {

                    var json = JObject.Parse(reader.ReadToEnd());

                    if (!json["statuses"].Any())
                    {
                        return "DONE";
                    }
                    Console.WriteLine(json["statuses"][0]["created_at"].ToString());
                    if (json["search_metadata"]["next_results"] != null)
                    {
                        UpdateTopicDetail(topicType, json, twitterDbLogic);
                        return json.GetValue("search_metadata").ToString();
                    }
                    else
                    {
                        return "DONE";
                    }


                }
            }
        }

        private void UpdateTopicDetail(int topicType, JObject json, TwitterLogic twitterLogic)
        {
            var topicRecord = twitterLogic.LoadRecordByTopicType<SearchModel>(topicType);
            var dailyCountList = topicRecord.dailyCountList;
            foreach (var jsonObject in json["statuses"])
            {
                var dateTime = DateTime.ParseExact(
                    jsonObject["created_at"].ToString(),
                    "ddd MMM dd HH:mm:ss +0000 yyyy",
                    CultureInfo.InvariantCulture);

                var date = dateTime.ToString("yyyy-MM-dd");
                var hasItem = false;
                foreach (var dailyCount in dailyCountList.Where(dailyCount => dailyCount.date == date))
                {
                    dailyCount.count++;
                    hasItem = true;
                }
                if (!hasItem)
                {
                    dailyCountList.Add(new DailyCountModel { date = date, count = 1 });
                }
            }

            topicRecord.dailyCountList = dailyCountList;
            topicRecord.nextResultUrl = json["search_metadata"]["next_results"].ToString();

            twitterLogic.UpsertRecordByTopic(topicType, topicRecord);
        }

    }
}