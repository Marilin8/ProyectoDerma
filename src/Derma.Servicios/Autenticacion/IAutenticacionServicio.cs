using Derma.Dominio.Enums;

namespace Derma.Servicios.Autenticacion;

public interface IAutenticacionServicio
{
    Task<ResultadoAutenticacion> IniciarSesionAsync(string email, string password);

    Task<ResultadoAutenticacion> RegistrarAsync(
        string nombreCompleto,
        string email,
        string password,
        RolUsuario rol);
}
