using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Data.Mapping;

public class RetornoJdpiMapping : IEntityTypeConfiguration<RetornoJdpi>
{
    public void Configure(EntityTypeBuilder<RetornoJdpi> builder)
    {
        builder.ToTable("retorno_jdpi");

        builder.HasKey(r => r.Id)
            .HasName("pk_retorno_jdpi");

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .HasColumnType("BIGSERIAL")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.TransacaoId)
            .HasColumnName("transacao_id")
            .HasColumnType("BIGINT")
            .IsRequired();

        builder.Property(r => r.RequisicaoJdpi)
            .HasColumnName("requisicao_jdpi")
            .HasColumnType("VARCHAR(500)")
            .IsRequired(false);

        builder.Property(r => r.RespostaJdpi)
            .HasColumnName("resposta_jdpi")
            .HasColumnType("VARCHAR(500)")
            .IsRequired(false);

        builder.Property(r => r.DataCriacao)
            .HasColumnName("data_criacao")
            .HasColumnType("DATE")
            .IsRequired();

        builder.Property(r => r.HoraCriacao)
            .HasColumnName("hora_criacao")
            .HasColumnType("TIME")
            .IsRequired();

        // Relationships
        builder.HasOne(r => r.Transacao)
            .WithMany(t => t.RetornosJdpi)
            .HasForeignKey(r => r.TransacaoId)
            .HasConstraintName("fk_retorno_jdpi_transacao")
            .OnDelete(DeleteBehavior.Cascade);
    }
}