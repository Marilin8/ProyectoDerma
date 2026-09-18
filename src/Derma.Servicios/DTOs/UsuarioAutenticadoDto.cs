using Derma.Dominio.Enums;

namespace Derma.Servicios.DTOs;

public record UsuarioAutenticadoDto(int Id, string NombreCompleto, string Email, RolUsuario Rol);
