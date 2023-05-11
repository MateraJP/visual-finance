using VisualFinanceiro.Negocios.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualFinanceiro.Negocios.Entities
{
    public class Carteira : Entity
    {
        [Column(TypeName = "nvarchar(120)")]
        public string Nome { get; set; }

        [Column(TypeName = "nvarchar(17)")]
        public TipoCarteira? TipoCarteira { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        public Situacao? Situacao { get; set; } = Enums.Situacao.EmElaboracao;
        [Column(TypeName = "nvarchar(120)")]
        public string Cor { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal TaxaRendimentoAoAno { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal TaxaJurosAoAno { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal Mensalidade { get; set; }
    }
}