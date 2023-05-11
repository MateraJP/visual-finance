using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Entities;
using VisualFinanceiro.Auth.Implementations;

namespace VisualFinanceiro.WebApi.Controllers
{
    [Authorize("grupo-despesa")]
    public class GrupoDespesaController : BaseCrudController<GrupoDespesa>
    {
        public GrupoDespesaController(ControleContaContext context) : base(context)
        {

        }

        protected override bool ValidaDelete(GrupoDespesa entity)
        {
            return entity.Situacao == Negocios.Enums.Situacao.EmElaboracao;
        }
    }
}