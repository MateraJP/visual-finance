using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualFinanceiro.Negocios.Entities
{
    public class PeriodoCarteira : Entity
    {

        public long CarteiraId { get; set; }

        public long PeriodoId { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal SaldoInicial { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal TotalEntrada { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal TotalSaida { get; set; }

        public virtual Periodo Periodo { get; set; }
        public virtual Carteira Carteira { get; set; }
    }
}