using static Mango.Web.Utility.DS;

namespace Mango.Web.Models
{
    public class RequestDto
    {
        public Apitype Apitype { get; set; } = Apitype.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
