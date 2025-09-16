using Microsoft.EntityFrameworkCore;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Data.Mapping;

namespace MS.Pix.MED.Infrastructure.Data;

public class MedDbContext : DbContext
{
    public MedDbContext(DbContextOptions<MedDbContext> options) : base(options)
    {
    }

    public DbSet<TipoInfracao> TiposInfracao { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }
    public DbSet<RetornoJdpi> RetornosJdpi { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply entity configurations
        modelBuilder.ApplyConfiguration(new TipoInfracaoMapping());
        modelBuilder.ApplyConfiguration(new TransacaoMapping());
        modelBuilder.ApplyConfiguration(new RetornoJdpiMapping());
    }
}