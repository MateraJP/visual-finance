using System.ComponentModel;

namespace VisualFinanceiro.Negocios.Enums
{
    [Description("Tipo de Carteira")]
    public enum TipoCarteira
    {
        [Description("Conta Corrente")]
        ContaCorrente,
        [Description("Conta Poupança")]
        ContaPoupanca,
        [Description("Conta de Investimentos")]
        ContaInvestimento,
        [Description("Cartão de Crédito")]
        CartaoCredito
    }
}