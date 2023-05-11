using System.ComponentModel;

namespace VisualFinanceiro.Negocios.Enums
{
    [Description("Situação do Período")]
    public enum SituacaoPeriodo
    {
        [Description("Em Elaboração")]
        EmElaboracao,
        [Description("Aberto")]
        Aberto,
        [Description("Consolidado")]
        Consolidado
    }
}