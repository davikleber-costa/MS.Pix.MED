using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Data.Mapping;

public class TipoInfracaoMapping : IEntityTypeConfiguration<TipoInfracao>
{
    public void Configure(EntityTypeBuilder<TipoInfracao> builder)
    {
        builder.ToTable("tipo_infracao");

        builder.HasKey(t => t.Id)
            .HasName("pk_tipo_infracao");

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .HasColumnType("BIGSERIAL")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Descricao)
            .HasColumnName("descricao")
            .HasColumnType("VARCHAR(300)")
            .IsRequired();

        builder.Property(t => t.Ativo)
            .HasColumnName("ativo")
            .HasColumnType("SMALLINT")
            .HasDefaultValue(1)
            .HasConversion(
                v => v ? (short)1 : (short)0,
                v => v == 1);

        // Relationships
        builder.HasMany(t => t.Transacoes)
            .WithOne(tr => tr.TipoInfracao)
            .HasForeignKey(tr => tr.TipoInfracaoId)
            .HasConstraintName("fk_transacao_tipo_infracao")
            .OnDelete(DeleteBehavior.Restrict);
    }
}