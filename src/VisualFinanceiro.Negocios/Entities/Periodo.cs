using VisualFinanceiro.Negocios.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualFinanceiro.Negocios.Entities
{
    public class Periodo : Entity
    {

        [Column(TypeName = "nvarchar(20)")]
        public string Codigo { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        public SituacaoPeriodo Situacao { get; set; }
    }
}