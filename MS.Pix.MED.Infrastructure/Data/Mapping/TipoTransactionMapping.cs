using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Data.Mapping;

public class TipoTransactionMapping : IEntityTypeConfiguration<TipoTransaction>
{
    public void Configure(EntityTypeBuilder<TipoTransaction> builder)
    {
        // Nome da tabela
        builder.ToTable("TbTipoTransaction");

        // Chave primária
        builder.HasKey(x => x.IdTpTransaction);
        builder.Property(x => x.IdTpTransaction)
            .HasColumnName("IdTpTransaction")
            .IsRequired();

        // Propriedades
        builder.Property(x => x.DsDescricao)
            .HasColumnName("DsDescricao")
            .HasColumnType("VARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.StAtivo)
            .HasColumnName("StAtivo")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        // Relacionamentos
        builder.HasMany(x => x.JdpiLogs)
            .WithOne()
            .HasForeignKey("TpTransaction")
            .OnDelete(DeleteBehavior.Restrict);
    }
}