using ErrorOr;
using MediatR;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.Application.Permissions.Commands.ModifyPermission
{
    public class ModifyPermissionCommandHandler : IRequestHandler<ModifyPermissionCommand, ErrorOr<Permiso>>
    {
        private readonly IPermisosRepository permisosRepository;
        private readonly ITipoPermisosRepository tipoPermisosRepository;
        private readonly IUnitOfWork unitOfWork;

        public ModifyPermissionCommandHandler(
            IPermisosRepository permisosRepository,
            ITipoPermisosRepository tipoPermisosRepository,
            IUnitOfWork unitOfWork)
        {
            this.permisosRepository = permisosRepository;
            this.tipoPermisosRepository = tipoPermisosRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Permiso>> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            var permiso = await permisosRepository.GetPermisosByIdAsync(request.PermisoId);
            if (permiso is null)
                return Error.NotFound("General.NotFound", $"No se encontró el permiso con ID {request.PermisoId}.");

            var tipoPermiso = await tipoPermisosRepository.GetTipoPermisoByIdAsync(request.TipoPermisoId);
            if (tipoPermiso is null)
                return Error.NotFound("General.NotFound", $"No se encontró el tipo de permiso con ID {request.TipoPermisoId}.");

            permiso.UpdatePermiso(request.NombreEmpleado, request.ApellidoEmpleado);
            permiso.SetTipoPermiso(tipoPermiso);

            await permisosRepository.UpdatePermisoAsync(request.PermisoId, permiso);
            await unitOfWork.CommitChangesAsync();

            return permiso;
        }
    }
}
