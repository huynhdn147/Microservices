namespace Mango.Web.Models
{
    public class JwtOptions
    {
        public string Isuser { get; set; } =string.Empty;
        public string Audience { get; set; } =string.Empty; 
        public string Secret { get; set; } = string.Empty;
    }
}
