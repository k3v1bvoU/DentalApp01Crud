using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace dentalApp02.Models
{
    public partial class PedidosDbContext : DbContext
    {
        public PedidosDbContext()
        {
        }

        public PedidosDbContext(DbContextOptions<PedidosDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Estado> Estados { get; set; } = null!;
        public virtual DbSet<Pedido> Pedidos { get; set; } = null!;
        public virtual DbSet<PedidoHistorial> PedidoHistorials { get; set; } = null!;
        public virtual DbSet<Protesi> Proteses { get; set; } = null!;
        public virtual DbSet<Tarea> Tareas { get; set; } = null!;
        public virtual DbSet<TareaHistorial> TareaHistorials { get; set; } = null!;
        public virtual DbSet<Transaccione> Transacciones { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ttt;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.ClienteId)
                    .ValueGeneratedNever()
                    .HasColumnName("ClienteID");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.Property(e => e.EstadoId)
                    .ValueGeneratedNever()
                    .HasColumnName("EstadoID");

                entity.Property(e => e.Fecha).HasColumnType("datetime");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.Property(e => e.PedidoId)
                    .ValueGeneratedNever()
                    .HasColumnName("PedidoID");

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.CreatedByUserId).HasMaxLength(450);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pedidos_Clientes");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pedidos_Estados");
            });

            modelBuilder.Entity<PedidoHistorial>(entity =>
            {
                entity.HasKey(e => e.HistorialId);

                entity.ToTable("PedidoHistorial");

                entity.Property(e => e.HistorialId)
                    .ValueGeneratedNever()
                    .HasColumnName("HistorialID");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.ModifiedByUserId).HasMaxLength(450);

                entity.Property(e => e.PedidoId).HasColumnName("PedidoID");

                entity.HasOne(d => d.Pedido)
                    .WithMany(p => p.PedidoHistorials)
                    .HasForeignKey(d => d.PedidoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PedidoHistorial_Pedidos");
            });

            modelBuilder.Entity<Protesi>(entity =>
            {
                entity.HasKey(e => e.ProtesisId);

                entity.ToTable("Protesis");

                entity.Property(e => e.ProtesisId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProtesisID");
            });

            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.Property(e => e.TareaId)
                    .ValueGeneratedNever()
                    .HasColumnName("TareaID");

                entity.Property(e => e.AssignedToUserId).HasMaxLength(450);

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.PedidoId).HasColumnName("PedidoID");

                entity.HasOne(d => d.Pedido)
                    .WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.PedidoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tareas_Pedidos");
            });

            modelBuilder.Entity<TareaHistorial>(entity =>
            {
                entity.HasKey(e => e.HistorialId);

                entity.ToTable("TareaHistorial");

                entity.Property(e => e.HistorialId)
                    .ValueGeneratedNever()
                    .HasColumnName("HistorialID");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.ModifiedByUserId).HasMaxLength(450);

                entity.Property(e => e.TareaId).HasColumnName("TareaID");

                entity.HasOne(d => d.Tarea)
                    .WithMany(p => p.TareaHistorials)
                    .HasForeignKey(d => d.TareaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TareaHistorial_Tareas");
            });

            modelBuilder.Entity<Transaccione>(entity =>
            {
                entity.HasKey(e => e.TransaccionId);

                entity.Property(e => e.TransaccionId)
                    .ValueGeneratedNever()
                    .HasColumnName("TransaccionID");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PedidoId).HasColumnName("PedidoID");

                entity.HasOne(d => d.Pedido)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.PedidoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transacciones_Pedidos");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
