using VisualFinanceiro.Negocios.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualFinanceiro.Negocios.Entities
{
    public class GrupoDespesa : Entity
    {

        [Column(TypeName = "nvarchar(20)")]
        public string Codigo { get; set; }

        [Column(TypeName = "nvarchar(120)")]
        public string Descricao { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        public Situacao Situacao { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Cor { get; set; }
    }
}