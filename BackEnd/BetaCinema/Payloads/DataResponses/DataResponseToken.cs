using Azure.Core;

namespace BetaCinema.Payloads.DataResponses
{
    public class DataResponseToken
    {
        //Xu ly thoi gian trong truy cap qua thi tu logout
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
