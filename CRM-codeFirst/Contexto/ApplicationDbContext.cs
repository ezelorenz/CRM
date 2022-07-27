using CRM_codeFirst.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CRM_codeFirst.Contexto
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions opciones) :base(opciones)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VendedorCliente>().HasKey(x => new { x.VendedorId, x.ClienteId });
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<VendedorCliente> VendedoresClientes { get; set; }
    }
}
