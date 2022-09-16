using EventrixAPI.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventrixAPI
{
    public class ApplicationDbContextPatch: IdentityDbContext
    {
        public ApplicationDbContextPatch(DbContextOptions<ApplicationDbContextPatch> options)
            :base(options)
        {
          
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
    }
}
