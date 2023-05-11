using System;

namespace VisualFinanceiro.Auth
{

    public class Authorized
    {
        public string AccessToken { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsExpired => Expiration < DateTime.Now;
    }
}
