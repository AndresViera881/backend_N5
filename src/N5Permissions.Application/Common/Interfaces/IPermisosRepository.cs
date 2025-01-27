using N5Permissions.Domain.Permissions;

namespace N5Permissions.Application.Common.Interfaces;

public interface IPermisosRepository
{
    Task<Permiso?> GetPermisosByIdAsync(int id);
    Task<List<Permiso>> GetPermisosAsync();
    Task UpdatePermisoAsync(int id, Permiso permiso);
    Task CreatePermisoAsync(Permiso permiso);
}