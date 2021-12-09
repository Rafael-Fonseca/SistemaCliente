using Microsoft.EntityFrameworkCore;

namespace SistemaCliente.Models
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        // HasQueryFilter Não esta funcionando com 
        //.IgnoreQueryFilters().FindAsync(id).AsNoTracking();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().Property(x => x.Id);
            modelBuilder.Entity<Cliente>().Property(x => x.Name);
            modelBuilder.Entity<Cliente>().Property(x => x.BirthDate);
            modelBuilder.Entity<Cliente>().Property(x => x.Gender);
            modelBuilder.Entity<Cliente>().HasQueryFilter(p => p.Active);
        }
        //*/
    }
}