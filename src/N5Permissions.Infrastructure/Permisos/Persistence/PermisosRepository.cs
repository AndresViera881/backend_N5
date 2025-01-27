using Microsoft.EntityFrameworkCore;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Domain.Permissions;
using N5Permissions.Infrastructure.Common.Persistence;

namespace N5Permissions.Infrastructure.Permisions.Persistence
{
    public class PermisosRepository : IPermisosRepository
    {
        private readonly ApplicationDbContext context;

        public PermisosRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Permiso?> GetPermisosByIdAsync(int id)
        {
            return await context.Permisos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Permiso>> GetPermisosAsync()
        {
            return await context.Permisos.Include(x => x.TipoPermiso).ToListAsync();
        }

        public async Task UpdatePermisoAsync(int id, Permiso permiso)
        {
            context.Permisos.Update(permiso);
        }

        public async Task CreatePermisoAsync(Permiso permiso)
        {
            await context.Set<Permiso>().AddAsync(permiso);
        }
    }
}
