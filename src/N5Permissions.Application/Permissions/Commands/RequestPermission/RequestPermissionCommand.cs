using ErrorOr;
using MediatR;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.Application.Permissions.Commands.RequestPermission;

public record RequestPermissionCommand(
    string NombreEmpleado,
    string ApellidoEmpleado,
    int TipoPermisoId
    ) : IRequest<ErrorOr<Permiso>>;