using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.Application.Permissions.Commands.RequestPermission;

public class RequestPermissionCommandHandler : IRequestHandler<RequestPermissionCommand, ErrorOr<Permiso>>
{
    private readonly IPermisosRepository permisosRepository;
    private readonly ITipoPermisosRepository tipoPermisosRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<RequestPermissionCommandHandler> logger;

    public RequestPermissionCommandHandler(
        IPermisosRepository permisosRepository,
        ITipoPermisosRepository tipoPermisosRepository,
        IUnitOfWork unitOfWork,
        ILogger<RequestPermissionCommandHandler> logger)
    {
        this.permisosRepository = permisosRepository;
        this.tipoPermisosRepository = tipoPermisosRepository;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }
    public async Task<ErrorOr<Permiso>> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
    {
        var tipoPermiso = await tipoPermisosRepository.GetTipoPermisoByIdAsync(request.TipoPermisoId);
        if (tipoPermiso is null)
        {
            return Error.NotFound(
                "General.NotFound", 
                $"No se encontró el tipo de permiso con ID {request.TipoPermisoId}, utilice un TipoPermisoId 1,2 o 3 (data semilla).");
        }

        var permiso = new Permiso(request.NombreEmpleado, request.ApellidoEmpleado, tipoPermiso);

        await permisosRepository.CreatePermisoAsync(permiso);
        await unitOfWork.CommitChangesAsync();
        logger.LogInformation("El Permiso fue guardado en la bd exitosamente.");
        return permiso;
    }
}