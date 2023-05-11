using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Entities;
using VisualFinanceiro.Auth.Implementations;

namespace VisualFinanceiro.WebApi.Controllers
{
    [Authorize("periodo")]
    public class PeriodoController : BaseCrudController<Periodo>
    {
        public PeriodoController(ControleContaContext context) : base(context)
        {

        }

        protected override bool ValidaDelete(Periodo entity)
        {
            return entity.Situacao == Negocios.Enums.SituacaoPeriodo.EmElaboracao;
        }
    }
}