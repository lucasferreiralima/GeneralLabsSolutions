using FluentValidation.Results;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.Domain.Mensageria;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.InfraStructure.Data
{
    public class AppDbContext : DbContext, IUnitOfWork
	{

		private readonly IMediatorHandler? _mediatorHandler;

        public AppDbContext(DbContextOptions<AppDbContext> options, 
                            IMediatorHandler? mediatorHandler = null)
			                : base(options) 
		{
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Produto> Produto { get; set; }
		public DbSet<CategoriaProduto> CategoriaProduto { get; set; }
		public DbSet<Cliente> Cliente { get; set; }
		public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Vendedor> Vendedor { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
		public DbSet<ItemPedido> ItemPedido { get; set; }
		public DbSet<Telefone> Telefone { get; set; }
		public DbSet<Pessoa> Pessoa { get; set; }
		public DbSet<Contato> Contato { get; set; }
		public DbSet<Voucher> Voucher { get; set; }

        public DbSet<EstadoDoItem> EstadoDoItem { get; set; }

        public DbSet<PessoaTelefone> PessoaTelefone { get; set; }
        public DbSet<PessoaContato> PessoaContato { get; set; }

        public DbSet<StatusDoItem> StatusDoItem { get; set; } 
        public DbSet<StatusDoItemIncompativel> StatusDoItemIncompativel { get; set; }
        public DbSet<HistoricoPedido> HistoricoPedido { get; set; }
        public DbSet<HistoricoItem> HistoricoItem { get; set; }

        // Novos modelos para QuadroKanban
        public DbSet<KanbanTask> KanbanTask { get; set; }
        public DbSet<Participante> Participante { get; set; }

        public DbSet<AgendaEventos> AgendaEventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Ignore<ValidationResult>();
			modelBuilder.Ignore<Event>();


            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
				e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
				property.SetColumnType("varchar(100)");

			foreach (var relationship in modelBuilder.Model.GetEntityTypes()
				.SelectMany(e => e.GetForeignKeys()))
				relationship.DeleteBehavior = DeleteBehavior.Restrict;

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}

		public async Task<bool> CommitAsync()
		{
			var sucesso = await base.SaveChangesAsync() > 0;
			if (sucesso)
				await _mediatorHandler!.PublicarEventos(this);
			return sucesso;
		}

    }

    #region: Todo: Guardar para quando for persistir Events

    public static class MediatorExtension
    {
        public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            // Exemplo de código padrão para publicar Domain Events
            var domainEntities = ctx.ChangeTracker
                .Entries<EntityBase>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any())
                .ToList();

            // Para cada entidade com eventos de domínio, publicar
            foreach (var entityEntry in domainEntities)
            {
                var events = entityEntry.Entity?.Notificacoes?.ToArray();
                entityEntry.Entity?.LimparEventos(); // se quiser limpar depois
                foreach (var domainEvent in events!)
                {
                    await mediator.PublicarEvento(domainEvent);
                }
            }
        }
    }

    #endregion
}
