using System.Windows;
using Derma.Servicios.DTOs;
using Derma.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Derma.UI.Views;

public partial class DashboardWindow : Window
{
    private readonly DashboardShellViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public DashboardWindow(DashboardShellViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;
        DataContext = _viewModel;

        _viewModel.SolicitaCerrarSesion += OnSolicitaCerrarSesion;
    }

    public void Inicializar(UsuarioAutenticadoDto usuario)
    {
        _viewModel.Inicializar(usuario);
    }

    private void OnSolicitaCerrarSesion(object? sender, EventArgs e)
    {
        var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
        loginWindow.Show();
        Close();
    }
}
