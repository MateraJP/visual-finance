using System;
using System.Text.Json.Serialization;

namespace VisualFinanceiro.Auth
{
    public class AuthResponse
    {
        [JsonIgnore]
        public string key { get; set; }
        public string access_token { get; set; }
        public string accessToken { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; } //ms
        public DateTime valid_until { get; set; }
        public DateTime created { get; set; }
        public DateTime expiration { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
        public bool authenticated { get; set; }
    }
}
