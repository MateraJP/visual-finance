using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Entities;
using VisualFinanceiro.Auth.Implementations;

namespace VisualFinanceiro.WebApi.Controllers
{
    [Authorize("grupo-lancamento")]
    public class GrupoLancamentoController : BaseCrudController<GrupoLancamento>
    {
        public GrupoLancamentoController(ControleContaContext context) : base(context)
        {

        }

        protected override bool ValidaDelete(GrupoLancamento entity)
        {
            return entity.Situacao == Negocios.Enums.Situacao.EmElaboracao;
        }
    }
}