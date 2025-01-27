using MediatR;
using ErrorOr;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.Application.Permissions.Queries.GetPermissions
{
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, ErrorOr<List<Permiso>>>
    {
        private readonly IPermisosRepository repository;

        public GetPermissionsQueryHandler(IPermisosRepository repository)
        {
            this.repository = repository;
        }
        public async Task<ErrorOr<List<Permiso>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var listaPermisos = await repository.GetPermisosAsync();

            return listaPermisos.Any()
                ? listaPermisos
                : Error.NotFound("General.NotFound", "No se encontró ningun permiso.");
        }
    }
}
