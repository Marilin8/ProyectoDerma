# ProyectoDerma

Sistema de gestión dermatológica (DERMA). Aplicación de escritorio WPF (.NET 8, MVVM) con EF Core y SQL Server 2022.

## Estructura

| Proyecto | Capa |
|---|---|
| `src/Derma.Dominio` | Entidades y enums |
| `src/Derma.Datos` | `DermaDbContext`, configuración EF Core y migraciones |
| `src/Derma.Servicios` | Lógica de negocio (autenticación con BCrypt) |
| `src/Derma.UI` | Aplicación WPF (login, registro, panel de secretaria, consultas) |
| `web-prototype/` | Prototipo web anterior (Node.js), solo de referencia |

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (incluido con Visual Studio 2022)
- Visual Studio 2022 con el workload **.NET desktop development** (o solo el SDK para usar la terminal)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) corriendo

## Puesta en marcha

1. **Clonar** el repositorio.

2. **Crear tus archivos de configuración locales** (ninguno se sube al repo, porque contienen la clave de la base de datos). Elige una contraseña fuerte para `sa` y úsala en ambos:

   ```bash
   cp .env.example .env
   cp src/Derma.UI/appsettings.local.json.example src/Derma.UI/appsettings.local.json
   ```

   - En `.env`, reemplaza `<define-una-clave-fuerte>` en `MSSQL_SA_PASSWORD`.
   - En `src/Derma.UI/appsettings.local.json`, reemplaza `<la-misma-clave-que-en-.env>` por esa misma clave.

3. **Levantar SQL Server en Docker** (desde la raíz del repo):

   ```bash
   docker compose up -d
   ```

   Espera ~20 segundos hasta que `docker ps` muestre el contenedor `derma-sqlserver` como `healthy`.

4. **Ejecutar la app.** Las dependencias NuGet se restauran solas y las migraciones se aplican automáticamente al arrancar (se crea la base `DermaDb`).

   - Visual Studio: abrir `Derma.sln`, establecer **Derma.UI** como proyecto de inicio y presionar **F5**.
   - Terminal:

     ```bash
     dotnet run --project src/Derma.UI
     ```

5. **Primer uso:** en la pantalla de login, usa "Regístrate aquí" para crear tu usuario (rol Secretaria, Doctor o Administrador).

## Base de datos (desarrollo)

- Contenedor: `derma-sqlserver`, puerto `1433`, usuario `sa`.
- La contraseña de `sa` se define en `.env` (`MSSQL_SA_PASSWORD`) y la app la lee desde `src/Derma.UI/appsettings.local.json` (que sobrescribe la cadena de `appsettings.json`). Deben coincidir.
- Si cambias la contraseña después de haber creado el contenedor, primero `docker compose down -v` (los datos se pierden).
- Los datos viven en el volumen Docker `derma-sqlserver-data`. Para empezar desde cero: `docker compose down -v`.
- Nueva migración: `dotnet ef migrations add <Nombre> --project src/Derma.Datos --startup-project src/Derma.Datos` (requiere `dotnet tool install --global dotnet-ef --version 8.0.11` y la variable de entorno `MSSQL_SA_PASSWORD` con la clave de `.env`).

> Esto es solo para desarrollo. En producción (`CLINICA-SRV`) se usa SQL Server Express instalado en el servidor, con un login de permisos acotados, no `sa`.
