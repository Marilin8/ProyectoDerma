using System.Text.RegularExpressions;
using Derma.Datos;
using Derma.Dominio.Entidades;
using Derma.Dominio.Enums;
using Derma.Servicios.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Derma.Servicios.Autenticacion;

public partial class AutenticacionServicio : IAutenticacionServicio
{
    private readonly DermaDbContext _dbContext;

    public AutenticacionServicio(DermaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultadoAutenticacion> IniciarSesionAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return ResultadoAutenticacion.Error("Usuario y contraseña son obligatorios.");
        }

        var emailNormalizado = email.Trim().ToLowerInvariant();
        var usuario = await _dbContext.Usuarios
            .FirstOrDefaultAsync(u => u.Email == emailNormalizado);

        if (usuario is null || !usuario.Activo || !BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
        {
            return ResultadoAutenticacion.Error("Usuario o contraseña incorrectos.");
        }

        return ResultadoAutenticacion.Ok(
            new UsuarioAutenticadoDto(usuario.Id, usuario.NombreCompleto, usuario.Email, usuario.Rol));
    }

    public async Task<ResultadoAutenticacion> RegistrarAsync(
        string nombreCompleto,
        string email,
        string password,
        RolUsuario rol)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return ResultadoAutenticacion.Error("Nombre, usuario y contraseña son obligatorios.");
        }

        if (!EmailRegex().IsMatch(email))
        {
            return ResultadoAutenticacion.Error("El usuario debe ser un email válido.");
        }

        if (password.Length < 6)
        {
            return ResultadoAutenticacion.Error("La contraseña debe tener al menos 6 caracteres.");
        }

        var emailNormalizado = email.Trim().ToLowerInvariant();
        var existe = await _dbContext.Usuarios.AnyAsync(u => u.Email == emailNormalizado);
        if (existe)
        {
            return ResultadoAutenticacion.Error("Ya existe una cuenta con ese usuario.");
        }

        var usuario = new Usuario
        {
            NombreCompleto = nombreCompleto.Trim(),
            Email = emailNormalizado,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Rol = rol,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _dbContext.Usuarios.Add(usuario);
        await _dbContext.SaveChangesAsync();

        return ResultadoAutenticacion.Ok(
            new UsuarioAutenticadoDto(usuario.Id, usuario.NombreCompleto, usuario.Email, usuario.Rol));
    }

    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")]
    private static partial Regex EmailRegex();
}
