using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualFinanceiro.Negocios.Entities
{
    public class LancamentoRecorrente : Entity
    {

        public long CarteiraId { get; set; }

        public long GrupoLancamentoId { get; set; }

        public long GrupoDespesaId { get; set; }

        public int DiaVencimento { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal Valor { get; set; }

        [Column(TypeName = "nvarchar(120)")]
        public string Descricao { get; set; }

        public virtual Carteira Carteira { get; set; }
        public virtual GrupoLancamento GrupoLancamento { get; set; }
        public virtual GrupoDespesa GrupoDespesa { get; set; }
    }
}