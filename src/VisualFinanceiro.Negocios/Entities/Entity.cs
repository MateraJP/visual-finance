
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualFinanceiro.Negocios.Entities
{
    public abstract class Entity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [System.Text.Json.Serialization.JsonIgnore()]
        public string DataKey { get; set; }
    }
}
