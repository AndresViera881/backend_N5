using ErrorOr;
using MediatR;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.Application.Permissions.Commands.ModifyPermission;

public record ModifyPermissionCommand(
    int PermisoId,
    string NombreEmpleado,
    string ApellidoEmpleado,
    int TipoPermisoId) : IRequest<ErrorOr<Permiso>>;