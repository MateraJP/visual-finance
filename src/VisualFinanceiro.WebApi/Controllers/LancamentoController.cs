using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Entities;
using VisualFinanceiro.Negocios.Enums;
using System.Linq;
using VisualFinanceiro.Auth.Implementations;

namespace VisualFinanceiro.WebApi.Controllers
{
    [Authorize("lancamento")]
    public class LancamentoController : BaseCrudController<Lancamento>
    {
        public LancamentoController(ControleContaContext context) : base(context)
        {

        }

        protected override bool ValidaCreate(Lancamento entity)
        {
            if (entity?.CarteiraId <= 0)
                ModelState.AddModelError("Campo Obrigatório", "CarteiraId");

            if (entity?.GrupoLancamentoId <= 0)
                ModelState.AddModelError("Campo Obrigatório", "GrupoLancamentoId");

            if (entity?.GrupoDespesaId <= 0)
                ModelState.AddModelError("Campo Obrigatório", "GrupoDespesaId");

            if (entity?.DataPrevisao == null)
                ModelState.AddModelError("Campo Obrigatório", "DataPrevisao");

            if (entity?.ValorPrevisao == null)
                ModelState.AddModelError("Campo Obrigatório", "ValorPrevisao");

            if (entity?.PeriodoId <= 0)
            {
                ModelState.AddModelError("Campo Obrigatório", "PeriodoId");
            }
            else
            {
                var periodo = db.Periodos.FirstOrDefault(p => p.Id == entity.PeriodoId);
                if (periodo?.Situacao == SituacaoPeriodo.Consolidado)
                    ModelState.AddModelError("Periodo", "Situação do Período não permite incluir registro");
            }

            return ModelState.IsValid;
        }

        protected override bool ValidaUpdate(Lancamento @new, Lancamento old)
        {
            if (@new?.CarteiraId <= 0)
                ModelState.AddModelError("Campo Obrigatório", "CarteiraId");

            if (@new?.PeriodoId <= 0)
                ModelState.AddModelError("Campo Obrigatório", "PeriodoId");

            if (@new?.GrupoLancamentoId <= 0)
                ModelState.AddModelError("Campo Obrigatório", "GrupoLancamentoId");

            if (@new?.GrupoDespesaId <= 0)
                ModelState.AddModelError("Campo Obrigatório", "GrupoDespesaId");

            if (@new?.DataPrevisao == null)
                ModelState.AddModelError("Campo Obrigatório", "DataPrevisao");

            if (@new?.ValorPrevisao == null)
                ModelState.AddModelError("Campo Obrigatório", "ValorPrevisao");

            if (old.Periodo?.Situacao == SituacaoPeriodo.Consolidado)
                ModelState.AddModelError("Periodo", "Situação do Período não permite alterar registro");

            return ModelState.IsValid;
        }

        protected override bool ValidaDelete(Lancamento entity)
        {
            if (entity.Periodo?.Situacao == SituacaoPeriodo.Consolidado)
                ModelState.AddModelError("Periodo", "Situação do Período não permite excluir registro");

            return ModelState.IsValid;
        }
    }
}