using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Derma.Dominio.Enums;
using Derma.Servicios.DTOs;

namespace Derma.UI.ViewModels;

public partial class DashboardShellViewModel : ObservableObject
{
    [ObservableProperty]
    private string _nombreCompleto = string.Empty;

    [ObservableProperty]
    private string _rolTexto = string.Empty;

    [ObservableProperty]
    private int _notificacionesCount = 3;

    [ObservableProperty]
    private object? _paginaActual;

    [ObservableProperty]
    private string _seccionActiva = "Inicio";

    public event EventHandler? SolicitaCerrarSesion;

    public void Inicializar(UsuarioAutenticadoDto usuario)
    {
        NombreCompleto = usuario.NombreCompleto;
        RolTexto = usuario.Rol switch
        {
            RolUsuario.Administrador => "Administrador",
            RolUsuario.Doctor => "Doctor",
            RolUsuario.Secretaria => "Secretaria",
            _ => usuario.Rol.ToString()
        };

        IrAInicio();
    }

    [RelayCommand]
    private void IrAInicio()
    {
        SeccionActiva = "Inicio";
        PaginaActual = new InicioViewModel(NombreCompleto);
    }

    [RelayCommand]
    private void IrAConsultas()
    {
        SeccionActiva = "Consultas";
        PaginaActual = new ConsultasViewModel();
    }

    [RelayCommand]
    private void IrACitas()
    {
        SeccionActiva = "Citas";
        PaginaActual = new PlaceholderViewModel("Citas", "Agendar y gestionar citas — próximamente.");
    }

    [RelayCommand]
    private void IrAMiCuenta()
    {
        SeccionActiva = "Mi Cuenta";
        PaginaActual = new PlaceholderViewModel("Mi Cuenta", "Próximamente.");
    }

    [RelayCommand]
    private void IrAConfiguracion()
    {
        SeccionActiva = "Configuración";
        PaginaActual = new PlaceholderViewModel("Configuración", "Próximamente.");
    }

    [RelayCommand]
    private void IrANotificaciones()
    {
        SeccionActiva = "Notificaciones";
        PaginaActual = new PlaceholderViewModel("Notificaciones", "Próximamente.");
    }

    [RelayCommand]
    private void IrAAyuda()
    {
        SeccionActiva = "Ayuda";
        PaginaActual = new PlaceholderViewModel("Ayuda", "Próximamente.");
    }

    [RelayCommand]
    private void CerrarSesion()
    {
        SolicitaCerrarSesion?.Invoke(this, EventArgs.Empty);
    }
}
