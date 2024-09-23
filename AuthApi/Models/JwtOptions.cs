namespace AuthApi.Models
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public int TokenExpireSeconds { get; set; }
        public string Audience { get; set; }

    }
}
