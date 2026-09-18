using System.IO;
using System.Windows;
using Derma.Datos;
using Derma.Servicios.Autenticacion;
using Derma.UI.ViewModels;
using Derma.UI.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Derma.UI;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.local.json", optional: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                Path.Combine(AppContext.BaseDirectory, "logs", "derma-.log"),
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var services = new ServiceCollection();

        var connectionString = configuration.GetConnectionString("DermaDb")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DermaDb'.");

        services.AddDbContext<DermaDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IAutenticacionServicio, AutenticacionServicio>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistroViewModel>();
        services.AddTransient<DashboardShellViewModel>();
        services.AddTransient<LoginWindow>();
        services.AddTransient<RegistroWindow>();
        services.AddTransient<DashboardWindow>();

        _serviceProvider = services.BuildServiceProvider();

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DermaDbContext>();
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "No se pudo conectar o migrar la base de datos al iniciar.");
            MessageBox.Show(
                "No se pudo conectar con la base de datos. Verifica que el contenedor de SQL Server esté corriendo.\n\n" + ex.Message,
                "DERMA — Error de conexión",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(-1);
            return;
        }

        var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
        loginWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}
