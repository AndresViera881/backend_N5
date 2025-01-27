using N5Permissions.Domain.Permissions;

namespace N5Permissions.Application.Common.Interfaces;

public interface ITipoPermisosRepository
{
    Task<TipoPermiso?> GetTipoPermisoByIdAsync(int id);
    Task CreateTipoPermisoAsync(TipoPermiso tipoPermiso);
}