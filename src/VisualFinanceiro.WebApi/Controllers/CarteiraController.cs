using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Entities;
using VisualFinanceiro.Auth.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace VisualFinanceiro.WebApi.Controllers
{
    [Authorize("carteira")]
    public class CarteiraController : BaseCrudController<Carteira>
    {
        public CarteiraController(ControleContaContext context) : base(context)
        {

        }

        [HttpPost, Route("ativar/{id}")]
        public virtual IActionResult Update(long id)
        {
            var set = db.Set<Carteira>();

            var local = set.Find(id);
            if (local == null)
                return NotFound();

            //if (!ValidaUpdate(entity, local))
            //    return ResponderValidationException();

            local.Situacao = Negocios.Enums.Situacao.Ativo;

            db.Entry(local).CurrentValues.SetValues(local);
            db.SaveChanges();
            return ResponderJsonResult(local);
        }

        [HttpPost, Route("lancamentos/{id}")]
        public virtual IActionResult Lancamentos(long id, [FromBody] SeachRequest request)
        {
            var set = db.Set<Carteira>();

            var local = set.Find(id);
            if (local == null)
                return NotFound();

            // TODO: Criar dapper para consultas
            var setLancamentos = db.Set<Lancamento>();
            var result = setLancamentos
                .Where(d => d.CarteiraId == local.Id)
                .OrderByDescending(d => d.DataPrevisao)
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Take(request.PageSize).ToList();
            //

            return ResponderJsonResult(result);
        }


        //protected override bool ValidaDelete(Carteira entity)
        //{
        //    return entity.Situacao == Negocios.Enums.Situacao.EmElaboracao;
        //}
    }
}