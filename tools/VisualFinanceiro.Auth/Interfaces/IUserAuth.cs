using System;

namespace VisualFinanceiro.Auth
{
    internal interface IUserAuth : IUser
    {
        internal bool IsAuth { get; set; }
        internal string InvalidMessage { get; set; }
        internal DateTime? BlockTemp { get; set; }
        internal int Tries { get; set; }
    }
}
