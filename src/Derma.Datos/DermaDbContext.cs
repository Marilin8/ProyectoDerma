using Derma.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Derma.Datos;

public class DermaDbContext : DbContext
{
    public DermaDbContext(DbContextOptions<DermaDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DermaDbContext).Assembly);
    }
}
