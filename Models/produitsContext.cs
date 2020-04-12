using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyStore.Models
{
    public partial class produitsContext : DbContext
    {
        public produitsContext()
        {
        }

        public produitsContext(DbContextOptions<produitsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Commands> Commands { get; set; }
        public virtual DbSet<Produits> Produits { get; set; }
        public virtual DbSet<Users> Users { get; set; }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commands>(entity =>
            {
                entity.HasKey(e => e.IdCom);

                entity.Property(e => e.IdCom).HasColumnName("idCom");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdAcheteur).HasColumnName("idAcheteur");

                entity.Property(e => e.IdProduit).HasColumnName("idProduit");

                entity.Property(e => e.Livraison)
                    .IsRequired()
                    .HasColumnName("livraison")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'non livrée')");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.IdAcheteurNavigation)
                    .WithMany(p => p.Commands)
                    .HasForeignKey(d => d.IdAcheteur)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Commands_Users");

                entity.HasOne(d => d.IdProduitNavigation)
                    .WithMany(p => p.Commands)
                    .HasForeignKey(d => d.IdProduit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Commands_produits");
            });

            modelBuilder.Entity<Produits>(entity =>
            {
                entity.ToTable("produits");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descprod)
                    .IsRequired()
                    .HasColumnName("descprod")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Discprod)
                    .HasColumnName("discprod")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Improd)
                    .IsRequired()
                    .HasColumnName("improd")
                    .HasColumnType("image");

                entity.Property(e => e.Namprod)
                    .IsRequired()
                    .HasColumnName("namprod")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Priprod)
                    .HasColumnName("priprod")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Stock)
                    .HasColumnName("stock")
                    .HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Admin)
                    .HasColumnName("admin")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Prenom)
                    .IsRequired()
                    .HasColumnName("prenom")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
