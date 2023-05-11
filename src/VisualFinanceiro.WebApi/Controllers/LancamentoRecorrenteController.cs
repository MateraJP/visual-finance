using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Entities;
using VisualFinanceiro.Auth.Implementations;

namespace VisualFinanceiro.WebApi.Controllers
{
    [Authorize("lancamento-recorrente")]
    public class LancamentoRecorrenteController : BaseCrudController<LancamentoRecorrente>
    {
        public LancamentoRecorrenteController(ControleContaContext context) : base(context)
        {

        }
    }
}