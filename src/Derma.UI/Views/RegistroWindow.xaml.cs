using System.Windows;
using Derma.UI.ViewModels;

namespace Derma.UI.Views;

public partial class RegistroWindow : Window
{
    private readonly RegistroViewModel _viewModel;

    public RegistroWindow(RegistroViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;

        _viewModel.RegistroExitoso += OnRegistroExitoso;
    }

    private async void BtnCrearCuenta_Click(object sender, RoutedEventArgs e)
    {
        await _viewModel.RegistrarAsync(TxtPassword.Password);
    }

    private void OnRegistroExitoso(object? sender, EventArgs e)
    {
        MessageBox.Show("Cuenta creada correctamente. Ahora puedes iniciar sesión.", "DERMA",
            MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }
}
