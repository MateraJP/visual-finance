using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using VisualFinanceiro.Auth;
using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Entities;
using VisualFinanceiro.Negocios.Enums;

namespace VisualFinanceiro.Negocios.Services
{
    public class AfterUserInsertService : IAfterUserInsert
    {
        private readonly ControleContaContext context;
        public AfterUserInsertService(ControleContaContext context) 
        {
            this.context = context;
        }

        public async Task AfterUserInsert(string dataKey)
        {
            //Criar estrutura básica de carteira e grupos com o Datakey gerado para o novo usuario
            await PopulateDefault(dataKey);
        }

        private async Task PopulateDefault(string datakey)
        {
            var gruposLancamentos = new List<GrupoLancamento>
            {
                new GrupoLancamento{Codigo="Entradas", Descricao="Entradas",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoLancamento{Codigo="Despesas", Descricao="Contas recorrentes",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoLancamento{Codigo="Compras", Descricao="Compras realizadas no período",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoLancamento{Codigo="Gastos", Descricao="Gastos realizados no período",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoLancamento{Codigo="Saldo", Descricao="Saldo do período anterior",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoLancamento{Codigo="Investimentos", Descricao="Valores investidos",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoLancamento{Codigo="Dividas", Descricao="Emprestimos, parcelamentos ou contas atrasadas",Situacao=Situacao.Ativo, DataKey=datakey},
            };
            gruposLancamentos.ForEach(s => context.GrupoLancamentos.Add(s));
            context.SaveChanges();

            var gruposDespesas = new List<GrupoDespesa>
            {
                new GrupoDespesa{Codigo="Casa", Descricao="Despesas com a residencia",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Mercado", Descricao="Compras no supermercado para casa",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Farmacia", Descricao="Compras na farmácia",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Padaria", Descricao="Compras na padaria",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Refeição", Descricao="Alimentação fora de casa",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Beer", Descricao="Gastos com cerveja em geral",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Games", Descricao="Gastos com jogos e consoles",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Moto", Descricao="Gastos com a moto",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Viagem", Descricao="Despesas de viagens",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Fundos", Descricao="Rendimentos recebidos em conta",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Rendimentos", Descricao="Rendimentos recebidos em conta",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Juros/Taxa", Descricao="Cobranças de Juros, multas e taxas sobre valores devidos",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Impostos", Descricao="Valores pagos em impostos",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Prolabore", Descricao="Valores pagos à sócios",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Lucro", Descricao="Lucros pagos à sócios",Situacao=Situacao.Ativo, DataKey=datakey},
                new GrupoDespesa{Codigo="Doação", Descricao="Doações realizadas à pessoas ou entidades",Situacao=Situacao.Ativo, DataKey=datakey},
            };
            gruposDespesas.ForEach(s => context.GrupoDespesas.Add(s));
            context.SaveChanges();

            var carteiras = new List<Carteira>
            {
                new Carteira{Nome="Itau",TipoCarteira=TipoCarteira.ContaCorrente, Situacao=Situacao.Ativo, Cor="#ec7000", TaxaJurosAoAno=new decimal(173.77), DataKey=datakey},
                new Carteira{Nome="Nubank",TipoCarteira=TipoCarteira.ContaCorrente, Situacao=Situacao.Ativo, Cor="#7b09c6",  TaxaRendimentoAoAno=new decimal(12.65), DataKey=datakey},
                new Carteira{Nome="Nucard",TipoCarteira=TipoCarteira.CartaoCredito, Situacao=Situacao.Ativo, Cor="#8305b4",  DataKey=datakey},
            };
            carteiras.ForEach(s => context.Carteiras.Add(s));
            context.SaveChanges();

            var periodos = new List<Periodo>
            {
                new Periodo{Codigo="202211",DataInicio=new DateTime(2022, 05, 01),DataFim=new DateTime(2022, 05, 31), Situacao=SituacaoPeriodo.Aberto, DataKey=datakey},
                new Periodo{Codigo="202212",DataInicio=new DateTime(2022, 06, 01),DataFim=new DateTime(2022, 06, 30), Situacao=SituacaoPeriodo.EmElaboracao, DataKey=datakey},
            };
            periodos.ForEach(s => context.Periodos.Add(s));
            context.SaveChanges();

            var lancamentos = new List<Lancamento>
            {
                new Lancamento
                {
                    CarteiraId = carteiras.FirstOrDefault().Id,
                    DataPrevisao = DateTime.Now.AddDays(5),
                    GrupoDespesaId = gruposDespesas.FirstOrDefault(d => d.Codigo == "Fundos").Id,
                    GrupoLancamentoId = gruposLancamentos.FirstOrDefault(d => d.Codigo == "Investimentos").Id,
                    PeriodoId = periodos.FirstOrDefault().Id,
                    ValorPrevisao = new decimal(2800),
                    Descricao = "Valor mensal apartamento",
                    DataKey = datakey
                },
                new Lancamento
                {
                    CarteiraId = carteiras.FirstOrDefault().Id,
                    DataPrevisao = DateTime.Now.AddDays(5).AddMonths(2),
                    GrupoDespesaId = gruposDespesas.FirstOrDefault(d => d.Codigo == "Fundos").Id,
                    GrupoLancamentoId = gruposLancamentos.FirstOrDefault(d => d.Codigo == "Investimentos").Id,
                    PeriodoId = periodos.FirstOrDefault().Id,
                    ValorPrevisao = new decimal(17000),
                    Descricao = "Valor anual apartamento",
                    DataKey = datakey
                },
            };
            lancamentos.ForEach(s => context.Lancamentos.Add(s));
            context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
