using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Data.Mapping;

public class TipoInfracaoMapping : IEntityTypeConfiguration<TipoInfracao>
{
    public void Configure(EntityTypeBuilder<TipoInfracao> builder)
    {
        builder.ToTable("TbMEDTipoInfracao", "dbo");
        
        builder.HasKey(x => x.IdTipoInfracao);
        
        builder.Property(x => x.IdTipoInfracao)
            .HasColumnName("IdTipoInfracao")
            .ValueGeneratedNever()
            .IsRequired();
            
        builder.Property(x => x.DsDescricao)
            .HasColumnName("DsDescricao")
            .HasColumnType("VARCHAR(300)")
            .IsRequired();
            
        builder.Property(x => x.StAtivo)
            .HasColumnName("StAtivo")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();
            
        // Relacionamento com Movimentacao
        builder.HasMany(x => x.Movimentacoes)
            .WithOne(x => x.TipoInfracao)
            .HasForeignKey(x => x.IdTipoInfracao)
            .HasConstraintName("FK_TbMEDMovimentacao_TipoInfracao")
            .OnDelete(DeleteBehavior.Restrict);
    }
}