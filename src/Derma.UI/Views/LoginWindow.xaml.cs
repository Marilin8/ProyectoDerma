using System.Windows;
using Derma.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Derma.UI.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public LoginWindow(LoginViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;
        DataContext = _viewModel;

        _viewModel.InicioSesionExitoso += OnInicioSesionExitoso;
    }

    private async void BtnIngresar_Click(object sender, RoutedEventArgs e)
    {
        await _viewModel.IniciarSesionAsync(TxtPassword.Password);
    }

    private void BtnIrARegistro_Click(object sender, RoutedEventArgs e)
    {
        var registroWindow = _serviceProvider.GetRequiredService<RegistroWindow>();
        registroWindow.Owner = this;
        registroWindow.ShowDialog();
    }

    private void OnInicioSesionExitoso(object? sender, EventArgs e)
    {
        var dashboard = _serviceProvider.GetRequiredService<DashboardWindow>();
        dashboard.Inicializar(_viewModel.UsuarioAutenticado!);
        dashboard.Show();
        Close();
    }
}
