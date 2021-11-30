using Microsoft.EntityFrameworkCore;

namespace Clientes.Models
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> TodoItems { get; set; }
    }
}