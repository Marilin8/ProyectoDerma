using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Derma.Datos;

/// Solo se usa en tiempo de diseño (dotnet ef migrations) para poder generar
/// migraciones sin depender del contenedor de DI de Derma.UI.
public class DermaDbContextFactory : IDesignTimeDbContextFactory<DermaDbContext>
{
    public DermaDbContext CreateDbContext(string[] args)
    {
        var saPassword = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD")
            ?? throw new InvalidOperationException(
                "Define la variable de entorno MSSQL_SA_PASSWORD (la misma clave del archivo .env) antes de usar dotnet ef.");
        var connectionString =
            $"Server=localhost,1433;Database=DermaDb;User Id=sa;Password={saPassword};TrustServerCertificate=True";

        var optionsBuilder = new DbContextOptionsBuilder<DermaDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new DermaDbContext(optionsBuilder.Options);
    }
}
