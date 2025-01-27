using Microsoft.EntityFrameworkCore;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Domain.Permissions;
using N5Permissions.Infrastructure.Common.Persistence;

namespace N5Permissions.Infrastructure.TipoPermisos.Persistence
{
    internal class TipoPermisosRepository : ITipoPermisosRepository
    {
        private readonly ApplicationDbContext context;

        public TipoPermisosRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<TipoPermiso?> GetTipoPermisoByIdAsync(int id)
        {
            return await context.TipoPermiso.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateTipoPermisoAsync(TipoPermiso tipoPermiso)
        {
            await context.Set<TipoPermiso>().AddAsync(tipoPermiso);
        }
    }
}
