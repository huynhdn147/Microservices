namespace Mango.Web.Utility
{
    public class DS
    {
        public static string CouponAPIBase { set; get; }
        public static string AuthAPIBase { set; get; }
		public static string ProductAPIBase { set; get; }

        public static string ShoppingCartAPIBase { set; get; }
        public static string RoleAdmin = "ADMIN";
        public static string RoleCustomer = "CUSTOMER";
        public static string TokenCookie = "JWTToken";
        public enum Apitype
        {
            GET,
            POST,
            PUT,
            DELETE
            
        }
    }
}
