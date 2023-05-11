namespace VisualFinanceiro.Negocios.Context
{
    public static class ControleContaInitializer
    {
        public static void Initialize(ControleContaContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}