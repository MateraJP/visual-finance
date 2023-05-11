using System.ComponentModel;

namespace VisualFinanceiro.Negocios.Enums
{
    [Description("Situação")]
    public enum Situacao
    {
        [Description("Em Elaboração")]
        EmElaboracao,
        [Description("Ativo")]
        Ativo,
        [Description("Inativo")]
        Inativo
    }
}