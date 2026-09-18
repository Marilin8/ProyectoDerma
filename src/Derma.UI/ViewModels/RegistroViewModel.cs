using CommunityToolkit.Mvvm.ComponentModel;
using Derma.Dominio.Enums;
using Derma.Servicios.Autenticacion;
using Derma.Servicios.DTOs;
using Serilog;

namespace Derma.UI.ViewModels;

public partial class RegistroViewModel : ObservableObject
{
    private readonly IAutenticacionServicio _autenticacionServicio;

    [ObservableProperty]
    private string _nombreCompleto = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private RolUsuario _rol = RolUsuario.Secretaria;

    [ObservableProperty]
    private string _mensajeError = string.Empty;

    [ObservableProperty]
    private bool _tieneError;

    [ObservableProperty]
    private bool _cargando;

    public IReadOnlyList<RolUsuario> RolesDisponibles { get; } =
        Enum.GetValues<RolUsuario>();

    public UsuarioAutenticadoDto? UsuarioRegistrado { get; private set; }

    public event EventHandler? RegistroExitoso;

    public RegistroViewModel(IAutenticacionServicio autenticacionServicio)
    {
        _autenticacionServicio = autenticacionServicio;
    }

    public async Task RegistrarAsync(string password)
    {
        TieneError = false;
        Cargando = true;

        try
        {
            var resultado = await _autenticacionServicio.RegistrarAsync(NombreCompleto, Email, password, Rol);

            if (!resultado.Exitoso)
            {
                MensajeError = resultado.MensajeError ?? "No se pudo crear la cuenta.";
                TieneError = true;
                return;
            }

            UsuarioRegistrado = resultado.Usuario;
            Log.Information("Cuenta creada para {Email} con rol {Rol}", Email, Rol);
            RegistroExitoso?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error al registrar la cuenta para {Email}", Email);
            MensajeError = "No se pudo conectar con la base de datos. Verifica tu conexión.";
            TieneError = true;
        }
        finally
        {
            Cargando = false;
        }
    }
}
