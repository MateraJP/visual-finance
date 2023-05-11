using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualFinanceiro.Negocios.Entities
{
    public class Lancamento : Entity
    {

        public long CarteiraId { get; set; }

        public long PeriodoId { get; set; }

        public long GrupoLancamentoId { get; set; }

        public long GrupoDespesaId { get; set; }

        public DateTime DataPrevisao { get; set; }

        public DateTime? DataEvetivado { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal ValorPrevisao { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal? ValorEfetivado { get; set; }

        [Column(TypeName = "nvarchar(120)")]
        public string Descricao { get; set; }

        public virtual Carteira Carteira { get; set; }
        public virtual GrupoLancamento GrupoLancamento { get; set; }
        public virtual GrupoDespesa GrupoDespesa { get; set; }
        public virtual Periodo Periodo { get; set; }
    }
}