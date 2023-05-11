using System;

namespace VisualFinanceiro.Auth
{
    internal class User : IUser, IUserAuth
    {
        public User() { }
        public User(string id) { Id = id; }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool? IsEmailValid { get; set; }
        public string[] claims { get; internal set; } = new string[] { "carteira", "grupo-despesa", "periodo" };
        public string ProfilePic { get; internal set; }

        bool IUserAuth.IsAuth { get; set; }
        string IUserAuth.InvalidMessage { get; set; }
        DateTime? IUserAuth.BlockTemp { get; set; }
        int IUserAuth.Tries { get; set; }
    }
}
