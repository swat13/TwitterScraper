using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Twitter.Data.Models;

namespace Twitter.Service.Interfaces
{
    [ServiceContract]
    public interface ITwitterApiManagement
    {
        [OperationContract]
        [FaultContract(typeof(MyFaults))]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        List<SearchModel> GetHotTopicsCount();
        void StartSearching();

    }
}