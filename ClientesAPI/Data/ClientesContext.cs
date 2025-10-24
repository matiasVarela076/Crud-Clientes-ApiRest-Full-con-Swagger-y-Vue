using ClientesAPI.Models;
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

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("CLIENTES");
                
                entity.HasKey(e => e.ID);
                entity.HasIndex(e => e.ID).IsUnique();
                entity.HasIndex(e => e.CUIT).IsUnique();
                
                entity.Property(e => e.NOMBRE).IsRequired().HasMaxLength(100);
                entity.Property(e => e.APELLIDO).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CUIT).IsRequired().HasMaxLength(20);
                entity.Property(e => e.TELEFONO).IsRequired().HasMaxLength(20);
                entity.Property(e => e.EMAIL).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DOMICILIO).HasMaxLength(200);             
                entity.Property(e => e.FECHA_CREACION).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.ACTIVO).IsRequired().HasDefaultValue(true);
                
            });
        }
    }
}
