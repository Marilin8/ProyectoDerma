using Derma.Servicios.DTOs;

namespace Derma.Servicios.Autenticacion;

public class ResultadoAutenticacion
{
    public bool Exitoso { get; }
    public string? MensajeError { get; }
    public UsuarioAutenticadoDto? Usuario { get; }

    private ResultadoAutenticacion(bool exitoso, string? mensajeError, UsuarioAutenticadoDto? usuario)
    {
        Exitoso = exitoso;
        MensajeError = mensajeError;
        Usuario = usuario;
    }

    public static ResultadoAutenticacion Ok(UsuarioAutenticadoDto usuario) => new(true, null, usuario);
    public static ResultadoAutenticacion Error(string mensaje) => new(false, mensaje, null);
}
