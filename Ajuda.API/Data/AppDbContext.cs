using Ajuda.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Ajuda.API
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoAjuda> TiposAjuda { get; set; }
        public DbSet<PedidoAjuda> PedidosAjuda { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("TB_USUARIO");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Nome).HasColumnName("NOME");
                entity.Property(e => e.Email).HasColumnName("EMAIL");
                entity.Property(e => e.Telefone).HasColumnName("TELEFONE");
                entity.Property(e => e.EhVoluntario).HasColumnName("EH_VOLUNTARIO");
            });

            modelBuilder.Entity<TipoAjuda>(entity =>
            {
                entity.ToTable("TB_TIPO_AJUDA");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Nome).HasColumnName("NOME");
                entity.Property(e => e.Descricao).HasColumnName("DESCRICAO");
            });

            modelBuilder.Entity<PedidoAjuda>(entity =>
            {
                entity.ToTable("TB_PEDIDO_AJUDA");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");
                entity.Property(e => e.TipoAjudaId).HasColumnName("TIPO_AJUDA_ID");

                entity.Property(e => e.Endereco).HasColumnName("ENDERECO");
                entity.Property(e => e.QuantidadePessoas).HasColumnName("QUANTIDADE_PESSOAS");
                entity.Property(e => e.DataHoraPedido).HasColumnName("DATA_HORA_PEDIDO");
                entity.Property(e => e.NivelUrgencia).HasColumnName("NIVEL_URGENCIA");

                entity.HasOne(p => p.Usuario)
                      .WithMany(u => u.PedidosAjuda)
                      .HasForeignKey(p => p.UsuarioId);

                entity.HasOne(p => p.TipoAjuda)
                      .WithMany()
                      .HasForeignKey(p => p.TipoAjudaId);
            });
        }
    }
}
