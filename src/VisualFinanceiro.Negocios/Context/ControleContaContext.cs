using VisualFinanceiro.Negocios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisualFinanceiro.Auth;

namespace VisualFinanceiro.Negocios.Context
{
    public class ControleContaContext : DbContext
    {
        public string DataKey { get; }
        public ControleContaContext(DbContextOptions<ControleContaContext> options, IDatakeyProvider authProvider) : base(options)
        {
            DataKey = authProvider.GetCurrentDataKey();
        }

        public DbSet<GrupoLancamento> GrupoLancamentos { get; set; }
        public DbSet<GrupoDespesa> GrupoDespesas { get; set; }
        public DbSet<Carteira> Carteiras { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<PeriodoCarteira> PeriodoCarteiras { get; set; }
        public DbSet<LancamentoRecorrente> LancamentoRecorrentes { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GrupoLancamento>().ToTable("GrupoLancamento").HasQueryFilter(b => EF.Property<string>(b, "DataKey") == DataKey);
            modelBuilder.Entity<GrupoDespesa>().ToTable("GrupoDespesa").HasQueryFilter(b => EF.Property<string>(b, "DataKey") == DataKey);
            modelBuilder.Entity<Carteira>().ToTable("Carteira").HasQueryFilter(b => EF.Property<string>(b, "DataKey") == DataKey);
            modelBuilder.Entity<Periodo>().ToTable("Periodo").HasQueryFilter(b => EF.Property<string>(b, "DataKey") == DataKey);
            modelBuilder.Entity<PeriodoCarteira>().ToTable("PeriodoCarteira").HasQueryFilter(b => EF.Property<string>(b, "DataKey") == DataKey);
            modelBuilder.Entity<LancamentoRecorrente>().ToTable("LancamentoRecorrente").HasQueryFilter(b => EF.Property<string>(b, "DataKey") == DataKey);
            modelBuilder.Entity<Lancamento>().ToTable("Lancamento").HasQueryFilter(b => EF.Property<string>(b, "DataKey") == DataKey);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType.BaseType == typeof(Enum))
                    {
                        var type = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                        var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;
                        property.SetValueConverter(converter);
                    }
                }
            }
        }

        public override int SaveChanges()
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return base.SaveChangesAsync(cancellationToken);
        }
    }

    public static class ContextExtension
    {
        public static void MarkWithDataKeyIfNeeded(this DbContext context, string accessKey)
        {
            var prop = typeof(Entity).GetProperty("DataKey");

            foreach (var entityEntry in context.ChangeTracker.Entries())
            {
                if (entityEntry.State == EntityState.Added)
                {
                    var hasDataKey = entityEntry.Entity as Entity;

                    if (hasDataKey != null && prop.GetValue(hasDataKey) == null)
                    {
                        // The DataKey is only updatedif its null
                        // This allow for the code to defining the DataKey
                        prop.SetValue(hasDataKey, accessKey);
                    }
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    var hasDataKey = entityEntry.Entity as Entity;
                    if (hasDataKey != null && prop.GetValue(hasDataKey) == null)
                    {
                        // Set back Datakey if modified
                        prop.SetValue(hasDataKey, entityEntry.OriginalValues["DataKey"]);
                    }
                }
            }
        }
    }
}