using ClientesAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientesAPI.Data
{
    public class ClientesContext : DbContext
    {
        public ClientesContext(DbContextOptions<ClientesContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la tabla Clientes
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.NOMBRE).IsRequired().HasMaxLength(100);
                entity.Property(e => e.APELLIDO).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CUIT).IsRequired().HasMaxLength(20);
                entity.Property(e => e.DOMICILIO).HasMaxLength(200);
                entity.Property(e => e.TELEFONO).HasMaxLength(20);
                entity.Property(e => e.EMAIL).HasMaxLength(100);
                entity.Property(e => e.ACTIVO).HasDefaultValue(true);
                entity.Property(e => e.FECHA_CREACION).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
