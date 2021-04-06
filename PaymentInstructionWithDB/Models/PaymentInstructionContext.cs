using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PaymentInstructionWithDB.Models
{
    public partial class PaymentInstructionContext : DbContext
    {
        public PaymentInstructionContext()
        {
        }

        public PaymentInstructionContext(DbContextOptions<PaymentInstructionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BillType> BillTypes { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<PaymentInstruction> PaymentInstructions { get; set; }
        public virtual DbSet<PaymentInstructionHistory> PaymentInstructionHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=HEM-PC\\SQLEXPRESS;Database=PaymentInstruction;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<BillType>(entity =>
            {
                entity.ToTable("BillType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<PaymentInstruction>(entity =>
            {
                entity.ToTable("PaymentInstruction");

                entity.Property(e => e.BeneficiaryBiccode)
                    .HasMaxLength(30)
                    .HasColumnName("BeneficiaryBICCode");

                entity.Property(e => e.BeneficiaryName).HasMaxLength(400);

                entity.HasOne(d => d.BillType)
                    .WithMany(p => p.PaymentInstructions)
                    .HasForeignKey(d => d.BillTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaymentIn__BillT__3D5E1FD2");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.PaymentInstructions)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaymentIn__Curre__3C69FB99");
            });

            modelBuilder.Entity<PaymentInstructionHistory>(entity =>
            {
                entity.ToTable("PaymentInstructionHistory");

                entity.Property(e => e.BeneficiaryBiccode)
                .HasMaxLength(30)
                .HasColumnName("BeneficiaryBICCode");

                entity.Property(e => e.BeneficiaryName).HasMaxLength(400);

                entity.HasOne(d => d.BillType)
                    .WithMany(p => p.PaymentInstructionHistories)
                    .HasForeignKey(d => d.BillTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaymentIn__BillT__4CA06362");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.PaymentInstructionHistories)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaymentIn__Curre__4D94879B");

                entity.HasOne(d => d.PaymentInstruction)
                    .WithMany(p => p.PaymentInstructionHistories)
                    .HasForeignKey(d => d.PaymentInstructionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaymentIn__Payme__4E88ABD4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
