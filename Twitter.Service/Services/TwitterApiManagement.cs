using System.Collections.Generic;
using Twitter.Controller.Controllers;
using Twitter.Data.Models;
using Twitter.Service.Interfaces;

namespace Twitter.Service.Services
{
    public class TwitterApiManagement : ITwitterApiManagement
    {
        public List<SearchModel> GetHotTopicsCount()
        {
            return new TwitterController().GetHotTopicsCount();
        }

        public void StartSearching()
        {
            new TwitterController().StartSearchingService();
        }
    }
}