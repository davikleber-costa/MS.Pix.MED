using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Data.Mapping;

public class MovimentacaoMapping : IEntityTypeConfiguration<Movimentacao>
{
    public void Configure(EntityTypeBuilder<Movimentacao> builder)
    {
        builder.ToTable("TbMEDMovimentacao", "dbo");
        
        builder.HasKey(x => x.IdMovimentacao);
        
        builder.Property(x => x.IdMovimentacao)
            .HasColumnName("IdMovimentacao")
            .ValueGeneratedOnAdd()
            .IsRequired();
            
        builder.Property(x => x.IdRelatoInfracao)
            .HasColumnName("IdRelatoInfracao")
            .HasColumnType("NVARCHAR(32)")
            .IsRequired(false);
            
        builder.Property(x => x.IdExtrato)
            .HasColumnName("IdExtrato")
            .HasColumnType("UNIQUEIDENTIFIER")
            .IsRequired();
            
        builder.Property(x => x.DtInclusao)
            .HasColumnName("DtInclusao")
            .HasColumnType("DATETIME")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();
            
        builder.Property(x => x.IdTipoInfracao)
            .HasColumnName("IdTipoInfracao")
            .IsRequired();
            
        builder.Property(x => x.FileBase64)
            .HasColumnName("FileBase64")
            .HasColumnType("VARCHAR(MAX)")
            .IsRequired(false);
            
        builder.Property(x => x.IdLog)
            .HasColumnName("IdLog")
            .IsRequired(false);
            
        // Relacionamento com TipoInfracao
        builder.HasOne(x => x.TipoInfracao)
            .WithMany(x => x.Movimentacoes)
            .HasForeignKey(x => x.IdTipoInfracao)
            .HasConstraintName("FK_TbMEDMovimentacao_TipoInfracao")
            .OnDelete(DeleteBehavior.Restrict);
            
        // Relacionamento com JdpiLog
        builder.HasOne(x => x.JdpiLog)
            .WithMany(x => x.Movimentacoes)
            .HasForeignKey(x => x.IdLog)
            .OnDelete(DeleteBehavior.Restrict);
    }
}