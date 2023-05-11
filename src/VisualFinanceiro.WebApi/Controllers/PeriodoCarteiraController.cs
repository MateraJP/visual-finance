using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Entities;
using VisualFinanceiro.Auth.Implementations;

namespace VisualFinanceiro.WebApi.Controllers
{
    [Authorize("periodo-carteira")]
    public class PeriodoCarteiraController : BaseCrudController<PeriodoCarteira>
    {
        public PeriodoCarteiraController(ControleContaContext context) : base(context)
        {

        }
    }
}