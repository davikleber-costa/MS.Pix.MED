using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Data.Mapping;

public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("transacao");

        builder.HasKey(t => t.Id)
            .HasName("pk_transacao");

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .HasColumnType("BIGSERIAL")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.TipoInfracaoId)
            .HasColumnName("tipo_infracao_id")
            .HasColumnType("BIGINT")
            .IsRequired();

        builder.Property(t => t.IdNotificacaoJdpi)
            .HasColumnName("id_notificacao_jdpi")
            .HasColumnType("CHAR(32)")
            .IsRequired();

        builder.Property(t => t.StatusRelatoJdpi)
            .HasColumnName("status_relato_jdpi")
            .HasColumnType("SMALLINT")
            .HasConversion(
                v => v ? (short)1 : (short)0,
                v => v == 1);

        builder.Property(t => t.GuidExtratoJdpi)
            .HasColumnName("guid_extrato")
            .HasColumnType("CHAR(32)")
            .IsRequired();

        builder.Property(t => t.CaminhoArquivo)
            .HasColumnName("caminho_arquivo")
            .HasColumnType("VARCHAR(200)")
            .IsRequired();

        // builder.Property(t => t.Agencia)
        //     .HasColumnName("agencia")
        //     .HasColumnType("CHAR(10)")
        //     .IsRequired(false);

        // builder.Property(t => t.Conta)
        //     .HasColumnName("conta")
        //     .HasColumnType("CHAR(20)")
        //     .IsRequired(false);

        // builder.Property(t => t.Observacao)
        //     .HasColumnName("observacao")
        //     .HasColumnType("VARCHAR(200)")
        //     .IsRequired(false);

        builder.Property(t => t.DataCriacao)
            .HasColumnName("data_criacao")
            .HasColumnType("DATE")
            .IsRequired();

        builder.Property(t => t.HoraCriacao)
            .HasColumnName("hora_criacao")
            .HasColumnType("TIME")
            .IsRequired();

        // Relationships
        builder.HasOne(t => t.TipoInfracao)
            .WithMany(ti => ti.Transacoes)
            .HasForeignKey(t => t.TipoInfracaoId)
            .HasConstraintName("fk_transacao_tipo_infracao")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.RetornosJdpi)
            .WithOne(r => r.Transacao)
            .HasForeignKey(r => r.TransacaoId)
            .HasConstraintName("fk_retorno_jdpi_transacao")
            .OnDelete(DeleteBehavior.Cascade);
    }
}