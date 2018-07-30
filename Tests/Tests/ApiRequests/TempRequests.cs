
namespace Tests.ApiRequests
{
    using RestSharp;

    public class TempRequests
    {
        public static RestRequest GetLearn()
        {
            var request = new RestRequest("/learn.json", Method.GET);                         
            return request;
        }
    }
}
