using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Data.Mapping;

public class JdpiLogMapping : IEntityTypeConfiguration<JdpiLog>
{
    public void Configure(EntityTypeBuilder<JdpiLog> builder)
    {
        builder.ToTable("TbMEDJdpiLog", "dbo");
        
        builder.HasKey(x => x.IdLog);
        
        builder.Property(x => x.IdLog)
            .HasColumnName("IdLog")
            .ValueGeneratedOnAdd()
            .IsRequired();
            
        builder.Property(x => x.DtLog)
            .HasColumnName("DtLog")
            .HasColumnType("DATETIME")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();
            
        builder.Property(x => x.IdRelatoInfracao)
            .HasColumnName("IdRelatoInfracao")
            .HasColumnType("NVARCHAR(32)")
            .IsRequired(false);
            
        builder.Property(x => x.StRelatoInfracao)
            .HasColumnName("StRelatoInfracao")
            .HasColumnType("SMALLINT")
            .IsRequired(false);
            
        builder.Property(x => x.IdExtrato)
            .HasColumnName("IdExtrato")
            .HasColumnType("UNIQUEIDENTIFIER")
            .IsRequired();
            
        builder.Property(x => x.ResponseJDPI)
            .HasColumnName("ResponseJDPI")
            .HasColumnType("VARCHAR(MAX)")
            .IsRequired(false);
            
        builder.Property(x => x.RequestJDPI)
            .HasColumnName("RequestJDPI")
            .HasColumnType("VARCHAR(MAX)")
            .IsRequired(false);
            
        builder.Property(x => x.TpTransaction)
            .HasColumnName("TpTransaction")
            .HasColumnType("BIT")
            .IsRequired(false);
            
        // Relacionamentos
        builder.HasMany(x => x.Movimentacoes)
            .WithOne(x => x.JdpiLog)
            .HasForeignKey(x => x.IdLog)
            .OnDelete(DeleteBehavior.Restrict);
    }
}