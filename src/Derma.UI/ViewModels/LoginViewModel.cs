using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Derma.Servicios.Autenticacion;
using Derma.Servicios.DTOs;
using Serilog;

namespace Derma.UI.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAutenticacionServicio _autenticacionServicio;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _mensajeError = string.Empty;

    [ObservableProperty]
    private bool _tieneError;

    [ObservableProperty]
    private bool _cargando;

    public UsuarioAutenticadoDto? UsuarioAutenticado { get; private set; }

    public event EventHandler? InicioSesionExitoso;

    public LoginViewModel(IAutenticacionServicio autenticacionServicio)
    {
        _autenticacionServicio = autenticacionServicio;
    }

    public async Task IniciarSesionAsync(string password)
    {
        TieneError = false;
        Cargando = true;

        try
        {
            var resultado = await _autenticacionServicio.IniciarSesionAsync(Email, password);

            if (!resultado.Exitoso)
            {
                MensajeError = resultado.MensajeError ?? "No se pudo iniciar sesión.";
                TieneError = true;
                return;
            }

            UsuarioAutenticado = resultado.Usuario;
            Log.Information("Inicio de sesión exitoso para {Email}", Email);
            InicioSesionExitoso?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error al iniciar sesión para {Email}", Email);
            MensajeError = "No se pudo conectar con la base de datos. Verifica tu conexión.";
            TieneError = true;
        }
        finally
        {
            Cargando = false;
        }
    }
}
